using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SocketTcpClient
{
    class Program : DealCards
    {
        // адрес и порт сервера, к которому будем подключаться
        static int port = 2115; // порт сервера
        static string address = "127.0.0.1"; // адрес сервера 176.196.126.194
        // Локальный адресс 127.0.0.1

        public static void RecieveTableCardsFromServer(Socket user)
        {
            //Остановился на получении данных клиента
            string CardSuit = "empty";//Данные для сообщения
            byte[] data = Encoding.Unicode.GetBytes(CardSuit);

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

            //Console.WriteLine("ответ сервера: " + builder.ToString());
            data = null;
            bytes = 0;
            builder.Clear();
        }
        
        

        //Получить данные
        public static void RecieveServerData(Socket user)
        {
            Console.Clear();
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

        public static int[] RecieveFlopeSuitServerData(Socket user)
        {
            
            string CardSuit = "empty"; //Данные для сообщения
            int enumCardSuit;
            byte[] dataSuit = Encoding.Unicode.GetBytes(CardSuit);

            // получаем ответ
            dataSuit = new byte[256]; // буфер для ответа
            StringBuilder builder = new StringBuilder();
            int bytes = 0; // количество полученных байт
            do
            {
                bytes = user.Receive(dataSuit, dataSuit.Length, 0);
                builder.Append(Encoding.Unicode.GetString(dataSuit, 0, bytes));
            }
            while (user.Available > 0);
            CardSuit = builder.ToString();
            enumCardSuit = Int32.Parse(CardSuit);//Значение масти карты
            Console.WriteLine("Значения масти: " + builder.ToString() + "\n");
            Console.WriteLine("Aboba");

            int[] arrayEnumCardSuit = new int[5] { enumCardSuit / 100, enumCardSuit / 10 % 10, enumCardSuit % 10, 0, 0 };

            dataSuit = null;
            bytes = 0;
            builder.Clear();

            return arrayEnumCardSuit;
        }
        public static int[] RecieveFlopeValueServerData(Socket user)
        {
            string CardValue = "empty"; //Данные для сообщения
            int enumCardValue;
            byte[] dataValue = Encoding.Unicode.GetBytes(CardValue);

            // получаем ответ
            dataValue = new byte[256]; // буфер для ответа
            StringBuilder builder = new StringBuilder();
            int bytes = 0; // количество полученных байт
            do
            {
                bytes = user.Receive(dataValue, dataValue.Length, 0);
                builder.Append(Encoding.Unicode.GetString(dataValue, 0, bytes));
            }
            while (user.Available > 0);
            CardValue = builder.ToString();
            enumCardValue = Int32.Parse(CardValue);//Значение масти карты

            int[] arrayEnumCardValue = new int[5] { enumCardValue / 100, enumCardValue / 10 % 10, enumCardValue % 10, 0, 0 };

            Console.WriteLine("Значения карты: " +enumCardValue + "\n");
            dataValue = null;
            bytes = 0;
            builder.Clear();

            return arrayEnumCardValue;
        }

        //отправить данные
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
            builder.Clear();
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

                //RecieveServerData(user);

                //Console.Clear()
                //ВЫВОД ФЛОПА

                DealCards dealCards = new();
                
                SendServerData(user);
                Console.Clear();

                int[] arrayEnumCardSuit = RecieveFlopeSuitServerData(user);
                Thread.Sleep(5000);
                int[] arrayEnumCardValue = RecieveFlopeValueServerData(user);
                for(int i = 0; i < 3; i++)
                { 
                    dealCards.GetHand(dealCards.TableCards[i], arrayEnumCardSuit, arrayEnumCardValue, i); 
                }

                dealCards.DisplayFlope();

                




            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.Read();
        }
    }
}