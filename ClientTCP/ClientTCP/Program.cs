using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

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
                    case '2': GetFileFromServer();
                        break;
                    case '3':
                        break;
                    default:
                        break;
                }
            } while (action.Key != ConsoleKey.D0);
        }

        private static void GetFileFromServer()
        {

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

        static void SendMessageFromSocket(int port)
        {
            
        }
    }
}
