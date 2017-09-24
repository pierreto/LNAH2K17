using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirHockeyServer.Entities
{
    public class ChatMessageEntity : Entity
    {
        public string Sender { get; set; }

        public string MessageValue { get; set; }

        public DateTime TimeStamp { get; set; }

        public ChatMessageEntity()
        {
        }
    }
}