using AirHockeyServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirHockeyServer.Events
{
    public class UserWaitingArgs
    {
        public UserEntity User { get; set; }
    }
}