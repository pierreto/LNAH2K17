using System;

namespace AirHockeyServer.Entities
{
    public class PasswordEntity : Entity
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public UserEntity User { get; set; }

        public string Password { get; set; }
    }
}