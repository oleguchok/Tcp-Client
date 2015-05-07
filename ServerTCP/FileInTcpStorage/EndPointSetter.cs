using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TcpStorageLibrary
{
    class EndPointSetter
    {
        private IPHostEntry ipHost = null;
        private IPAddress ipAddress = null;
        private IPEndPoint ipEndPoint = null;

        public IPHostEntry IPHost { get { return ipHost; } }
        public IPAddress IPAddress { get { return ipAddress; } }
        public IPEndPoint IPEndPoint { get { return ipEndPoint; } }

        public EndPointSetter(IPHostEntry _ipHost, IPAddress _ipAddress, int port)
        {
            ipHost = _ipHost;
            ipAddress = _ipAddress;
            ipEndPoint = new IPEndPoint(ipAddress, port);
        }
    }
}
