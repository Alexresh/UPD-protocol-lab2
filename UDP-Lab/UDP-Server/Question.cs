using System;
using System.Collections.Generic;
using System.Text;

namespace UDP_Server
{
    class Question
    {
        public string Quest { get; set; }
        public string[] Answer { get; set; }
        public int RightAnswer { get; set; }
        public Question(string question,string[] answers,int rightAnswer) 
        {
            Quest = question;
            Answer = answers;
            RightAnswer = rightAnswer;
        }
    }
}
