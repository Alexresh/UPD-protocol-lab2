using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UDP_Server
{
    class Initial
    {
        private static List<IPEndPoint> clients = new List<IPEndPoint>();
        static void Main(string[] args)
        {
            UdpClient server = new UdpClient(5002);

            
            server.Client.ReceiveTimeout = 1000 * 10;

            try
            {
                IPEndPoint RemoteIpEndPoint = null;
                while (true) 
                {
                    byte[] receiveBytes = server.Receive(ref RemoteIpEndPoint);
                    clients.Add(RemoteIpEndPoint);
                }
                //string returnData = Encoding.UTF8.GetString(receiveBytes);
            }
            catch (SocketException)
            {
                
            }
            byte[] firstQuestion = Encoding.UTF8.GetBytes("Было 3 козла.\nСколько?");
            clients.ForEach(client=> { server.Send(firstQuestion, firstQuestion.Length, client); });
            Console.ReadLine();
        }

    }
}
