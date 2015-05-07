using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TcpStorageLibrary
{
    public class EndPointSetter
    {
        private IPHostEntry ipHost = null;
        private IPAddress ipAddress = null;
        private IPEndPoint ipEndPoint = null;

        public IPHostEntry IPHost { get { return ipHost; } }
        public IPAddress IPAddress { get { return ipAddress; } }
        public IPEndPoint IPEndPoint { get { return ipEndPoint; } }

        public EndPointSetter(IPHostEntry _ipHost, int addressNumber, int port)
        {
            ipHost = _ipHost;
            ipAddress = ipHost.AddressList[addressNumber];
            ipEndPoint = new IPEndPoint(ipAddress, port);
        }
    }
}
