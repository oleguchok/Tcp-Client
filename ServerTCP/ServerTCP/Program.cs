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
                    Stream stream = new MemoryStream(buf, 0, buf.Length);
                    object obj = formatter.Deserialize(stream);
                    if (obj is FileInTcpStorage)
                    {
                        byte[] response = GetFileBytes(obj as FileInTcpStorage);
                        handler.Send(response);
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
    }
}
