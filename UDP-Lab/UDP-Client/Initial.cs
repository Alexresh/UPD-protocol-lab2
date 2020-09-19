using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UDP_Client
{
    class Initial
    {
        static void Main()
        {

            UdpClient client = new UdpClient();
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1337);
            Console.Title = "Виктор и на";

            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes("0");
                client.Send(bytes, bytes.Length, endPoint);
                IPEndPoint server = null;
                byte[] receiveTimeOut = client.Receive(ref server);
                //получаем время получения след. вопроса
                client.Client.ReceiveTimeout = int.Parse(Encoding.UTF8.GetString(receiveTimeOut))+1000;
                byte[] receivePlayer = client.Receive(ref server);
                string player = Encoding.UTF8.GetString(receivePlayer);
                Console.WriteLine(player);

                bool game = true;
                while (game)
                {
                    byte[] receiveBytes = client.Receive(ref server);
                    string quest = Encoding.UTF8.GetString(receiveBytes);
                    if (quest.Equals("end game"))
                    {
                        game = false;
                    }
                    else
                    {
                        Console.WriteLine(quest);
                        try
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("Ваш выбор[0-4]: ");
                            Console.ResetColor();
                            string answer = Console.ReadLine();
                            if (!int.TryParse(answer, out int intAnswer))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Ответ не удовлетворяет условию answer принадлежит [1,4], ваш ответ не засчитан!");
                                Console.ResetColor();
                                answer = "0";
                            }
                            else
                            {
                                if (intAnswer > 4 || intAnswer < 1) 
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Ответ не удовлетворяет условию answer принадлежит [1,4], ваш ответ не засчитан!");
                                    Console.ResetColor();
                                    answer = "0";
                                }
                                
                            }
                            byte[] byteAnswer = Encoding.UTF8.GetBytes(answer);
                            client.Send(byteAnswer, byteAnswer.Length, server);
                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                    }
                }
                Console.WriteLine("Игра закончилась!");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(Encoding.UTF8.GetString(client.Receive(ref server)));
            }
            catch (SocketException)
            {
                Console.WriteLine("Пакет не получен");
            }
            finally
            {
                client.Close();
            }
            Console.ReadLine();
        }

    }
}
