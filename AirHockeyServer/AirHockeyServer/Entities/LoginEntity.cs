using System;

namespace AirHockeyServer.Entities
{
    public class LoginEntity : Entity
    {
        public Guid LoginId { get; }
        public UserEntity User { get; set; }
        public string Password { get; set; }

        public LoginEntity()
        {
        }
    }
}