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
        public static int[] arrayEnumCardSuit = new int[5];
        public static int[] arrayEnumCardValue = new int[5];
        public static int stage = 1;//stage 2 handCard, stage 3 flope and turn, stage 4 river
        public static DealCards dealCards = new();
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

        public static void RecieveCards(Socket user)
        {
            string Cards = "empty"; //Данные для сообщения
            int enumCard;
            byte[] dataSuit = Encoding.Unicode.GetBytes(Cards);

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
            Cards = builder.ToString();
            
            enumCard = Int32.Parse(Cards);//Значение масти карты
            //
            if (stage == 1)
            {//311209
                //Console.WriteLine(Cards[0]);
                arrayEnumCardSuit[0] = enumCard / 100000;//3
                arrayEnumCardSuit[1] = enumCard / 10000 % 10;//1
                arrayEnumCardValue[0] = enumCard / 100 % 100;//12
                arrayEnumCardValue[1] = enumCard % 100;//09
            }
            else if(stage == 2)
            {//302 12 23 11
                arrayEnumCardSuit[0] = enumCard / 100000000;//3
                arrayEnumCardSuit[1] = enumCard / 10000000 % 10;//0
                arrayEnumCardSuit[2] = enumCard / 1000000 % 10;//2
                arrayEnumCardValue[0] = enumCard / 10000 % 100;//12
                arrayEnumCardValue[1] = enumCard / 100 % 100;//23
                arrayEnumCardValue[2] = enumCard % 100;//11
                
            }
            else if(stage == 3)
            {//212
                //Console.WriteLine(Cards);
                arrayEnumCardSuit[3] = enumCard / 100;//2
                arrayEnumCardValue[3] = enumCard % 100;//12
            }
            else if(stage == 4)
            {//315
                //Console.WriteLine(Cards);
                arrayEnumCardSuit[4] = enumCard / 100;//3
                arrayEnumCardValue[4] = enumCard % 100;//15
            }
            // Console.WriteLine(enumCard);

            enumCard = 0;
            Cards = null;
            dataSuit = null;
            bytes = 0;
            builder.Clear();
        }
        public static void sleep()
        {
            Thread.Sleep(500);
        }
        public static void CloseSocket(Socket user)
        {
            // закрываем сокет
            user.Shutdown(SocketShutdown.Both);
            user.Close();
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
        static void Main(string[] args)
        {
            Console.Title = "Блэйк Джек";

            ConsoleColor[] colors = (ConsoleColor[])ConsoleColor.GetValues(typeof(ConsoleColor));
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.Clear();

            Console.BufferWidth = 120;
            Console.WindowWidth = Console.BufferWidth;
            Console.WindowHeight = 40;
            Console.BufferHeight = Console.WindowHeight;

            try
            {
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

                Socket user = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // подключаемся к удаленному хосту
                user.Connect(ipPoint);
                
                //RecieveServerData(user);
                ////RecieveServerData(user);
                //SendServerData(user);
                Console.Clear();
                //stage 1
                RecieveCards(user);
                dealCards.DisplayPlayerCard();
                stage++;//2

                RecieveCards(user);
                dealCards.DisplayFlope();
                //stage = 2

                stage++;//3
                RecieveCards(user);
                dealCards.DisplayTurn();//ОШИБКА ОТПРАВЛЕНИЯ ТЕРНА

                stage++;//4
                RecieveCards(user);
                dealCards.DisplayRiver();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.Read();
        }
    }
}