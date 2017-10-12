using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirHockeyServer.Pocos
{
    public class LoginPoco: Poco
    {
        public override int? Id { get; set; }

        public int UserId { get; private set; }

        public string Password { get; private set; }
    }
}