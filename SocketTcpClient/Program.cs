using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace SocketTcpClient
{
    class Program
    {
        // адрес и порт сервера, к которому будем подключаться
        static int port = 2115; // порт сервера
        static string address = "127.0.0.1"; // адрес сервера 176.196.126.194
        // Локальный адресс 127.0.0.1

        public static void RecieveServerData(Socket user)
        {
            string message = "empty";//Данные для сообщения
            byte[] data = Encoding.Unicode.GetBytes(message);

            // получаем ответ
            data = new byte[256]; // буфер для ответа
            StringBuilder builder = new StringBuilder();
            int bytes = 0; // количество полученных байт

            do
            {
                bytes = user.Receive(data, data.Length, 0);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (user.Available > 0);
            Console.WriteLine("ответ сервера: " + builder.ToString());
            data = null;
            bytes = 0;
            builder.Clear();
        }

        public static void SendServerData(Socket user)
        {
            string message = "empty";//Данные для сообщения
            byte[] data = Encoding.Unicode.GetBytes(message);

            // получаем ответ
            data = new byte[256]; // буфер для ответа
            StringBuilder builder = new StringBuilder();
            int bytes = 0; // количество полученных байт

            Console.Write("Введите сообщение:");
            message = Console.ReadLine();
            data = Encoding.Unicode.GetBytes(message);
            user.Send(data);
            data = null;
            bytes = 0;
        }

        public static void CloseSocket(Socket user)
        {
            // закрываем сокет
            user.Shutdown(SocketShutdown.Both);
            user.Close();
        }

        static void Main(string[] args)
        {
            try
            {
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

                Socket user = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // подключаемся к удаленному хосту
                user.Connect(ipPoint);


                RecieveServerData(user);
                SendServerData(user);

                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.Read();
        }
    }
}