using System;
using System.Collections.Generic;
using System.Net;

namespace UDP_Server
{
    class Client
    {
        public IPEndPoint IpPort { get; set; }
        public List<Answer> Answers = new List<Answer>();
        public string Name { get; set; }
        public decimal AwesomeBalls { get; set; }
        public Client(IPEndPoint ipPort) 
        {
            IpPort = ipPort;
        }

        public void AddAnswer(int choose) 
        {
            Answers.Add(new Answer(choose));
        }
        
    }
}
