using System;

namespace AirHockeyServer.Entities
{
    public class LoginEntity : Entity
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}