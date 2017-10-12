using System;

namespace AirHockeyServer.Entities
{
    public class LoginEntity : Entity
    {
        public int LoginId { get; }
        public UserEntity User { get; set; }
        public string Password { get; set; }

        public LoginEntity()
        {
        }
    }
}