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

namespace ClientTCP
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            ConsoleKeyInfo action;
            do
            {
                ShowHelp();
                action = Console.ReadKey();
                switch (action.KeyChar)
                {
                    case '1': ShowFilesOnServer();
                        break;
                    case '2':
                        FileInTcpStorage fileInfo = GetFileInfo();
                        byte[] response = new byte[fileInfo.SizeOfBlock];
                        SendMessage(11000, MessageToRequestFile(fileInfo),ref response);
                        WriteFileInUploads(response, fileInfo);
                        break;
                    case '3':
                        break;
                    default:
                        break;
                }
            } while (action.Key != ConsoleKey.D0);
        }

        private static void SendMessage(int port, byte[] msg,ref byte[] response)
        {   
            EndPointSetter endPoint = new EndPointSetter(Dns.GetHostEntry("localhost"),
                                                            0, port);
            Socket sender = new Socket(endPoint.IPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            sender.Connect(endPoint.IPEndPoint);            

            sender.Send(msg);

            sender.Receive(response);           

            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
            Console.WriteLine("File was downloaded!");
        }

        private static void WriteFileInUploads(byte[] buf, FileInTcpStorage fileInfo)
        {
            FileStream file = new FileStream(@"D:\GitHub\Tcp-Client\Uploads\" + fileInfo.Name, FileMode.Create);
            file.Write(buf, 0, buf.Length);
            file.Close();
        }

        private static byte[] MessageToRequestFile(FileInTcpStorage fileInfo)
        {
            Stream streamFileInfo = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(streamFileInfo, fileInfo);
            streamFileInfo.Position = 0;
            byte[] msg = new byte[streamFileInfo.Length];
            streamFileInfo.Read(msg, 0, (int)streamFileInfo.Length);
            streamFileInfo.Close();

            return msg;
        }

        private static void ShowFilesOnServer()
        {
            
        }

        private static void ShowHelp()
        {
            Console.WriteLine("\tTcp Client");
            Console.WriteLine("1 - Show Files on server");
            Console.WriteLine("2 - Get File");
            Console.WriteLine("3 - Show Help");
            Console.WriteLine("0 - exit");
            Console.WriteLine("Press key:");
        }

        private static FileInTcpStorage GetFileInfo()
        {
            bool isError = false;
            FileInTcpStorage file = new FileInTcpStorage();
            Console.WriteLine("Enter File name");
            file.Name = Console.ReadLine();
            do
            {
                try
                {
                    Console.WriteLine("Enter Offset");
                    file.Offset = Convert.ToInt32(Console.ReadLine());
                    isError = false;
                }
                catch
                {
                    Console.WriteLine("Incorrect input");
                    isError = true;
                }
            } while (isError != false);
            do
            {
                try
                {
                    Console.WriteLine("Enter Size of Block");
                    file.SizeOfBlock = Convert.ToInt32(Console.ReadLine());
                    isError = false;
                }
                catch
                {
                    Console.WriteLine("Incorrect input");
                    isError = true;
                }
            } while (isError != false);
            
            return file;
        }
    }
}
