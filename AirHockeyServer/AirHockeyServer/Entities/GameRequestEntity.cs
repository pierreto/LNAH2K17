using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirHockeyServer.Entities
{
    public class GameRequestEntity
    {
        public UserEntity Sender { get; set; }

        public UserEntity Recipient { get; set; }

        public bool IsAccept { get; set; }
    }
}