using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpStorageLibrary
{
    [Serializable]
    public class FileInTcpStorage
    {
        public string Name { get; set; }
        public int Offset { get; set; }
        public int SizeOfBlock { get; set; }
    }
}
