using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webGui.ServiceStack.Plugins
{
    using System.Net;
    using System.Net.Sockets;

    using global::ServiceStack.WebHost.Endpoints;

    public class WebSocketPlugin : IPlugin, IWebSocketPlugin
    {
        static TcpListener _server = new TcpListener(IPAddress.Loopback,80);

        public void Register(IAppHost appHost)
        {
            if(appHost.TryResolve<IWebSocketPlugin>() == null)
                appHost.Register<IWebSocketPlugin>(this);

            _server.BeginAcceptTcpClient(ClientLoop, null);
        }

        private void ClientLoop(IAsyncResult result)
        {

            _server.BeginAcceptTcpClient(ClientLoop, null);
        }

    }

    public interface IWebSocketPlugin
    {
    }
}