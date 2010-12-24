using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ruleEngine.ruleItems.Starts
{
    public class imapChecker
    {
        private int cmdNum;
        private NetworkStream tcpStream;
        private StreamWriter myWriter;
        private StreamReader myReader;
        public bool newMail;

        public void makeImapChecker(string server, int port, string username, string password, bool useSSL)
        {
            imapConnect(server, port, useSSL);
            cmdNum = 1;
            using (tcpStream)
            {
                imapLogin(username, password);
                imapSelect("inbox");
                newMail = imapSearch();
            }
        }

        public imapChecker(emailOptions options)
        {
            makeImapChecker(options.serverName, options.portNum, options.username, options.password,
                                   options.useSSL);
        }

        private bool imapSearch()
        {
            sendCmd("search unseen");

            bool responded = false;
            String loginResponse;
            bool newItems = false;
            while (!responded)
            {
                loginResponse = myReader.ReadLine();

                String[] responseParts = (loginResponse.Split(' '));
                if (responseParts.Length > 0)
                {
                    if (responseParts[0] == "a" + cmdNum.ToString("000"))
                    {
                        if (responseParts.Length < 3)
                            throw new Exception("Server sent incomprehensible response to SEARCH -  '" + loginResponse +
                                                "'");

                        if (responseParts[1].ToUpper() == "OK")
                        {
                            responded = true;
                        }
                        else
                        {
                            if (responseParts[1].ToUpper() == "NO")
                                throw new Exception("Searching for new mail failed.");
                            else if (responseParts[1].ToUpper() == "BAD")
                                throw new Exception("Server failed to parse SEARCH command");
                            else
                                throw new Exception("Server gave unrecognised response ('" + responseParts[1] +
                                                    "') to SEARCH");
                        }
                    }
                    if (responseParts[1] == "SEARCH")
                    {
                        if (responseParts.Length > 2)
                            newItems = true;
                    }
                }
            }

            return newItems;
        }

        private void imapSelect(string folder)
        {
            sendCmd("select " + folder);

            bool responded = false;
            String loginResponse;
            while (!responded)
            {
                loginResponse = myReader.ReadLine();

                String[] responseParts = (loginResponse.Split(' '));
                if (responseParts.Length > 0 && responseParts[0] == "a" + cmdNum.ToString("000"))
                {
                    if (responseParts.Length < 3)
                        throw new Exception("Server sent incomprehensible response to SELECT -  '" + loginResponse + "'");

                    if (responseParts[1].ToUpper() == "OK")
                        responded = true;
                    else
                    {
                        if (responseParts[1].ToUpper() == "NO")
                            throw new Exception("No such mailbox '" + folder + "' exists, or is not accessible");
                        else if (responseParts[1].ToUpper() == "BAD")
                            throw new Exception("Server failed to parse SELECT command");
                        else
                            throw new Exception("Server gave unrecognised response ('" + responseParts[1] + "') to SELECT");
                    }
                }
            }

        }

        private void imapLogin(string username, string password)
        {
            // todo: check for LOGINDISABLED capability
            sendCmd("LOGIN " + username + " " + password);

            String loginResponse = myReader.ReadLine();

            String[] answerChunks = loginResponse.Split(' ');
            if (answerChunks[1].ToUpper() == "NO")
                throw new Exception("bad login. Check username / password and retry. (server line '" + loginResponse + "')");
            if (answerChunks[1].ToUpper() == "BAD")
                throw new Exception("Failure sending username/password. Check login, hope it isn't a bug (server line '" + loginResponse + "')");
            if (!(answerChunks[1].ToUpper() == "OK"))
                throw new Exception("Server response to login not recognised (as OK, NO, or BAD) (server line '" + loginResponse + "')");
        }

        private void sendCmd(string command)
        {
            cmdNum++;
            StringBuilder toSend = new StringBuilder();
            toSend.Append("a");
            toSend.Append(cmdNum.ToString("000"));
            toSend.Append(" ");
            toSend.Append(command);
            myWriter.WriteLine(toSend);
        }

        private void imapConnect(string server, int port, bool useSSL)
        {
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            TcpClient myClient = new TcpClient(server, port);
            tcpStream = myClient.GetStream();

            if (useSSL)
            {
                SslStream mySSLStream = new SslStream(tcpStream, false, new RemoteCertificateValidationCallback(foo));
                mySSLStream.AuthenticateAsClient(server);
                myWriter = new StreamWriter(mySSLStream);
                myWriter.AutoFlush = true;
                myReader = new StreamReader(mySSLStream);
            }
            else
            {
                myWriter = new StreamWriter(tcpStream);
                myWriter.AutoFlush = true;
                myReader = new StreamReader(tcpStream);                
            }

            char[] charIn = new char[1];

            string lineIn = myReader.ReadLine();

            // todo: support for PREAUTH
            if (lineIn.StartsWith("* BYE"))
                throw new Exception("Mail server didn't want us to connect, asked us to disconnect");
            if (!lineIn.StartsWith("* OK"))
                throw new Exception("Mail server didn't give the required connection banner");
        }

        // todo: !!!!
        private bool foo(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;    // ULTRAHAX
        }
    }
}
