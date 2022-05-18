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
        
        public static void RecieveTableCardsData(Socket user, Card card)
        {//ОСТАНОВИЛСЯ НА ТОМ ЧТО НАДО БЫЛО НЕ СТРИНГ А ИНТ ПЕРЕДАВАТЬ Я В АХУЕ
            string CardValue = null;
            string CardSuit = null;

            byte[] dataSuit = Encoding.Unicode.GetBytes(CardSuit);
            byte[] dataValue = Encoding.Unicode.GetBytes(CardValue);

            dataSuit = new byte[256]; // буфер для ответа
            dataValue = new byte[256];

            StringBuilder builderSuit = new StringBuilder();
            StringBuilder builderValue = new StringBuilder();
            int bytes = 0; // количество полученных байт
            for (int i = 0; i < 3; i++)
            {
                do
                {
                    bytes = user.Receive(dataValue, dataValue.Length, 0);
                    builderSuit.Append(Encoding.Unicode.GetString(dataSuit, 0, bytes));
                }
                while (user.Available > 0);

                CardSuit = builderSuit.ToString();

                bytes = 0;
                do
                {
                    bytes = user.Receive(dataSuit, dataSuit.Length, 0);
                    builderValue.Append(Encoding.Unicode.GetString(dataSuit, 0, bytes));
                }
                while (user.Available > 0);

                CardValue = builderValue.ToString();

                int x = 0;

                DisplayFlope(CardSuit, CardValue, i);
            }
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

        public static void RecieveFlopeSuitServerData(Socket user)
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
            dataSuit = null;
            bytes = 0;
            builder.Clear();
        }
        public static void RecieveFlopeValueServerData(Socket user)
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

            Console.WriteLine("Значения карты: " +enumCardValue + "\n");
            dataValue = null;
            bytes = 0;
            builder.Clear();
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
                //ПРИСВАИВАНИЕ ТИПА

                SendServerData(user);
                Console.Clear();
                RecieveFlopeSuitServerData(user);
                Thread.Sleep(5000);
                RecieveFlopeValueServerData(user);

                DealCards dealCards = new();
                

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.Read();
        }
    }
}