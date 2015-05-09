using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using TcpStorageLibrary;

namespace ServerTCP
{
    class Program
    {
        private static int port = 11000;
        private static byte[] List = Encoding.Default.GetBytes("List");

        static void Main(string[] args)
        {
            EndPointSetter endPoint = new EndPointSetter(Dns.GetHostEntry("localhost"),
                                                         0, port);
            Socket sListener = new Socket(endPoint.IPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try 
            {
                sListener.Bind(endPoint.IPEndPoint);
                sListener.Listen(2);

                while(true)
                {
                    Socket handler = sListener.Accept();
                    byte[] buf = new byte[handler.ReceiveBufferSize];
                    handler.Receive(buf);
                    BinaryFormatter formatter = new BinaryFormatter();
                    if (IsItListRequest(buf))
                    {
                        List<string> files = GetListOfFilesOnServer();
                        Stream str = new MemoryStream();
                        formatter.Serialize(str, files);
                        str.Position = 0;
                        byte[] msg = new byte[str.Length];
                        str.Read(msg, 0, (int)str.Length);
                        str.Close();
                        handler.Send(msg);
                    }
                    else
                    {
                        Stream stream = new MemoryStream(buf, 0, buf.Length);
                        object obj = formatter.Deserialize(stream);
                        if (obj is FileInTcpStorage)
                        {
                            byte[] response = GetFileBytes(obj as FileInTcpStorage);
                            handler.Send(response);
                        }
                    }
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static byte[] GetFileBytes(FileInTcpStorage file)
        {
            byte[] buf = new byte[file.SizeOfBlock];
            FileStream fs = new FileStream(@"D:\GitHub\Tcp-Client\Files\" + file.Name, FileMode.Open);
            fs.Position = file.Offset;
            fs.Read(buf, 0, file.SizeOfBlock);
            fs.Close();
            return buf;
        }

        private static bool IsItListRequest(byte[] buf)
        {
            if (buf[0] == List[0] && buf[1] == List[1] && buf[2] == List[2] && buf[3] == List[3])
                return true;
            else
                return false;
        }

        private static List<string> GetListOfFilesOnServer()
        {
            string[] allFiles = Directory.GetFiles(@"D:\GitHub\Tcp-Client\Files\");
            return allFiles.ToList();
        }
    }
}
