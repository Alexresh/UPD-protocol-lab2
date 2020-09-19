using System;

namespace UDP_Server
{
    class Answer
    {
        public int Choose { get; set; }
        public bool IsFirst { get; set; }
        public Answer(int choose) 
        {
            Choose = choose;
        }
    }
}
