using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceGraphique.Entities
{
    public class ChatMessage
    {
        public ChatMessage()
        {
        }


        public ChatMessage(string sender, string messageValue, DateTime timeStamp)
        {
            Sender = sender;
            MessageValue = messageValue;
            TimeStamp = timeStamp;
        }

        public string Sender { get; set; }

        public string MessageValue { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}
