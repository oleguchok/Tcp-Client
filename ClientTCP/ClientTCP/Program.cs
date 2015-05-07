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
                    case '2': GetFileFromServer(11000);
                        break;
                    case '3':
                        break;
                    default:
                        break;
                }
            } while (action.Key != ConsoleKey.D0);
        }

        private static void GetFileFromServer(int port)
        {
            EndPointSetter endPoint = new EndPointSetter(Dns.GetHostEntry("localhost"),
                                                            0, port);
            Socket sender = new Socket(endPoint.IPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            sender.Connect(endPoint.IPEndPoint);
            FileInTcpStorage fileInfo = GetFileInfo();

            Stream streamFileInfo = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(streamFileInfo, fileInfo);

            byte[] msg = new byte[streamFileInfo.Length];
            streamFileInfo.Read(msg,0,(int)streamFileInfo.Length);
            streamFileInfo.Close();

            byte[] response = new byte[fileInfo.SizeOfBlock];

            sender.Send(msg);

            sender.Receive(response);

            FileStream file = new FileStream(@"../Files/" + fileInfo.Name, FileMode.Create);
            file.Write(response, 0, response.Length);
            file.Close();

            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
            Console.WriteLine("File was downloaded!");
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
