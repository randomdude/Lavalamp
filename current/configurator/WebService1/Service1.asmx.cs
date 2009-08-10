using System;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Web.Services;
using System.Xml.Serialization;

namespace WebService1
{
    /// <summary>
    /// Take an XML object describing a node, and kick off an mplab build for it.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Service1 : System.Web.Services.WebService
    {
        // Create a new node, and serialize it, as an example of XML formatting.
        [WebMethod]
        public node ExampleNode()
        {
            node exampleNode = new node {name = "Test node"};

            generic_digital_in exampleSensorIn;
            generic_digital_out exampleSensorOut;

            exampleSensorIn = new generic_digital_in { id = 1, pin = new ClassPin(enumPort.PORTB, 1) };
            exampleNode.sensor.Add(exampleSensorIn);

            exampleSensorOut = new generic_digital_out { id = 2, pin = new ClassPin(enumPort.PORTC, 1) };
            exampleNode.sensor.Add(exampleSensorOut);

            exampleSensorIn = new generic_digital_in { id = 3, pin = new ClassPin(enumPort.PORTA, 2) };
            exampleNode.sensor.Add(exampleSensorIn);

            return exampleNode;
        }

        // given a configuration file, kick off a build.
        // TODO: analyse security of this!
        [WebMethod]
        public buildResults doBuild(String configXml)
        {
            // Get source and build sensorcfg.h from XML
            String tempFolder = getSourceAndBuildConfig(configXml);

            // Load the project file manually, so we can carry out any pre-build tasks, since
            // mplab doesn't include these in the things it exports to the Makefile (!!)
            String projectFile = tempFolder + "\\" + ConfigurationManager.AppSettings["buildPath"] + "\\" + ConfigurationManager.AppSettings["projectFile"];
            project thisProj = new project(projectFile);
            try
            {
                thisProj.doPreBuild();
            } catch(Exception e) {
                return new buildResults() { failReason = e.Message, success = false };                 
            }

            // Now execute 'make'. N0te that we execute it from the project directory, after
            // adding the path to the MPASM suite to the PATH. mlpab generates some totally
            // cracked-out makefiles - at least as of version 8.1 - so you best cross your
            // fingers and hope it all goes OK...
            Process  makeProcess = new Process();
            makeProcess.StartInfo.Arguments = ConfigurationManager.AppSettings["extraMakeArgs"];
            makeProcess.StartInfo.WorkingDirectory = tempFolder + "\\" + ConfigurationManager.AppSettings["buildPath"];
            makeProcess.StartInfo.EnvironmentVariables["PATH"] = makeProcess.StartInfo.EnvironmentVariables["PATH"] + ";" +
                                                                 ConfigurationManager.AppSettings["mpasmSuitePath"];
            makeProcess.StartInfo.FileName = "make";
            makeProcess.StartInfo.ErrorDialog = false;
            makeProcess.StartInfo.UseShellExecute = false;
            makeProcess.StartInfo.RedirectStandardError = true;
            makeProcess.StartInfo.RedirectStandardOutput = true;
            makeProcess.StartInfo.RedirectStandardInput = true;
            makeProcess.Start();
            if (!makeProcess.WaitForExit(int.Parse(ConfigurationManager.AppSettings["buildTimeout"])))
            {
                return new buildResults()
                    {failReason = "Timeout on build", success = false}; 
            }

            // Make our object to return and fill stdout and success flags.
            buildResults toret = new buildResults();
            toret.stdout  = makeProcess.StandardError.ReadToEnd();
            // Set sucess or not 
            String target = tempFolder + "\\" + ConfigurationManager.AppSettings["buildPath"] + "\\" +
                            ConfigurationManager.AppSettings["target"];
            if (File.Exists(target))
                toret.success = true;
            else
                toret.success = false;

            // Add .error files
            FileInfo [] errorFiles =
                new DirectoryInfo(tempFolder + "\\" + ConfigurationManager.AppSettings["buildPath"]).GetFiles("*.err");
            int n = 0;
            toret.errors = new errorFile[errorFiles.Length];
            foreach (FileInfo thisFile in errorFiles)
            {
                StreamReader errReader = new StreamReader(thisFile.FullName);
                toret.errors[n] = new errorFile();
                toret.errors[n].name = thisFile.Name;
                toret.errors[n].contents = new String(errReader.ReadToEnd().ToCharArray());
                errReader.Close();
                n++;
            }

            return toret;
        }

        private String getSourceAndBuildConfig(string xmlToBuild)
        {
            String tempDir = System.IO.Path.GetTempFileName();

            File.Delete(tempDir);
            Directory.CreateDirectory(tempDir);

            // Grab latest code from svn
            Process svn = new Process();
            svn.StartInfo.UseShellExecute = false;
            svn.StartInfo.EnvironmentVariables["SVN_SSH"] = ConfigurationManager.AppSettings["plinkPath"];
            svn.StartInfo.FileName = "svn";
            svn.StartInfo.Arguments = " checkout " + ConfigurationManager.AppSettings["svnPath"] + @" """ + tempDir + @"""";

            svn.Start();
            svn.WaitForExit(10000);
            if (!svn.HasExited )
                throw new Exception("timed out waiting for checkout");

            // Create new config!
            String configFile = buildConfigFile(xmlToBuild);
            StreamWriter configWriter = new StreamWriter(tempDir + "\\" + ConfigurationManager.AppSettings["configFile"]);
            configWriter.Write(configFile);
            configWriter.Close();

            return tempDir;
        }

        private String buildConfigFile(string xmlToBuild)
        {
            if (xmlToBuild == null)
                throw new NullReferenceException();

            XmlSerializer deser = new XmlSerializer(typeof (node));
            node toBuild = (node)deser.Deserialize(new StringReader(xmlToBuild));

            StringBuilder code = new StringBuilder();
            code.Append( toBuild.generateCode());

            foreach (Object currentSensor in toBuild.sensor)
            {
                Type T = currentSensor.GetType();
                if (T == typeof(generic_digital_in))
                    code.Append(((generic_digital_in)currentSensor).generateCode());
                else if (T == typeof(generic_digital_out))
                    code.Append(((generic_digital_out)currentSensor).generateCode());
                else
                    throw new Exception("unrecognised sensor type " + T.ToString());
            }

            return code.ToString();
        }
    }
}
