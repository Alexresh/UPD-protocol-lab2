using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UDP_Client
{
    class Initial
    {
        static void Main(string[] args)
        {

            UdpClient client = new UdpClient();
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5002);


            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes("1");

                client.Send(bytes, bytes.Length, endPoint);
                IPEndPoint server = null;
                byte[] receiveBytes = client.Receive(ref server);
                Console.WriteLine(Encoding.UTF8.GetString(receiveBytes));
                

            }
            catch (Exception ex)
            {
                Console.WriteLine("Возникло исключение: " + ex.ToString() + "\n  " + ex.Message);
            }
            finally 
            { 
                client.Close();
            }
            Console.ReadLine();
        }
        
    }
}
