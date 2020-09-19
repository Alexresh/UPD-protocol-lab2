using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UDP_Server
{
    class Initial
    {
        private static readonly UdpClient server = new UdpClient(1337);
        private static bool first = false;
        private static bool fastGame = false;
        private static readonly List<Client> clients = new List<Client>();
        private static readonly List<Question> questions = new List<Question>()
        {
            new Question("Было 2 козла.\nСколько?",new string[]{"1","3","4","Да"},4),
            new Question("Почему?",new string[]{"потому что","кто здесь?","я здаюсь","Да"},1),
            new Question("JavaScript: alert('1' + true) ?",new string[]{"1","1true","2","Да"},2),
            new Question("DateTime.Now=?",new string[]{DateTime.Now.ToString(),"DateTime.Now()","2020","Да"},2),
            new Question("В треугольнике ABC  AB = BC. Из точки E на стороне AB опущен перпендикуляр ED на BC. Оказалось, что  AE = ED.  Найдите угол DAC.",new string[]{ "35°", "45°", "55°", "Да"},2),
        };
        static void Main(string[] args)
        {
            if (args.Length != 0)
            {
                fastGame = true;
            }
            
            server.Client.ReceiveTimeout = fastGame==true ? 10*1000 : 60*1000;
            Console.Title = "Server";
            Console.WriteLine($"Ожидание игроков {server.Client.ReceiveTimeout/1000} секунд");

            try
            {
                IPEndPoint RemoteIpEndPoint = null;
                while (true)
                {
                    byte[] receiveBytes = server.Receive(ref RemoteIpEndPoint);

                    clients.Add(new Client(RemoteIpEndPoint));
                    SendMessage($"Ты теперь в теле Игрок{clients.Count}", clients[^1]);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Игрок {clients.Count} присоеденился");
                    Console.ResetColor();
                    clients[^1].Name = "Игрок" + clients.Count;

                }
            }
            catch (SocketException)
            {

            }
            questions.ForEach(quest =>
            {
                string answers = "\n";
                for (int i = 0; i < quest.Answer.Length; i++)
                {
                    answers += (i + 1).ToString() + ". " + quest.Answer[i] + "\n";
                }
                SendMessage(quest.Quest + answers);
                Console.WriteLine($"Вопрос отправлен({quest.Quest.Replace('\n',' ')})");
                first = false;
                try
                {
                    while (true) ReceiveAnswer();
                }
                catch (SocketException) { }
                catch (Exception) { throw; }
            });
            SendMessage("end game");//игра закончилась
            CalcResults();
            Client winner = ChooseWinner();
            string winMessage = winner == null ? "SkyNet win!" : $"Победитель {winner.Name}, набравший {winner.AwesomeBalls} баллов!";
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(winMessage);
            SendMessage(winMessage);
            Console.ReadLine();
        }

        private static Client ChooseWinner()
        {
            decimal maxBall = clients.Max(client => client.AwesomeBalls);
            if (maxBall == 0) 
            {
                return null;
            }
            return clients.FirstOrDefault(client => client.AwesomeBalls == maxBall);
        }

        private static void CalcResults()
        {
            clients.ForEach(client => 
            {
                decimal clientScore=0;
                for (int i = 0; i < client.Answers.Count; i++)
                {
                    if (client.Answers[i].Choose == questions[i].RightAnswer) 
                    {
                        clientScore += 1.0m;
                        if (client.Answers[i].IsFirst) clientScore += 0.01m;
                    }
                }
                client.AwesomeBalls = clientScore;
            });


        }

        private static void ReceiveAnswer()
        {
            server.Client.ReceiveTimeout = fastGame == true ? 10 * 1000 : 20 * 1000;
            IPEndPoint remoteIp = null;
            byte[] receiveBytes = server.Receive(ref remoteIp);
            int receiveAnswer = int.Parse(Encoding.UTF8.GetString(receiveBytes));
            Client client = clients.FirstOrDefault(client => client.IpPort.Port == remoteIp.Port);
            if (client != null) 
            {
                client.AddAnswer(receiveAnswer);
                if (first == false) 
                {
                    first = true;
                    client.Answers[^1].IsFirst = true;
                }
                

            }
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(client.Name+" ответил на вопрос, его ответ: "+receiveAnswer);
            Console.ResetColor();
        }

        private static void SendMessage(string message)
        {
            byte[] byteMessage = Encoding.UTF8.GetBytes(message);
            clients.ForEach(client => { server.Send(byteMessage, byteMessage.Length, client.IpPort); });
        }

        private static void SendMessage(string message, Client client)
        {
            byte[] byteMessage = Encoding.UTF8.GetBytes(message);
            server.Send(byteMessage, byteMessage.Length, client.IpPort);
        }
    }
}
