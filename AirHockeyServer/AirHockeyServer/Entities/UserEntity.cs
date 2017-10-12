using System;

namespace AirHockeyServer.Entities
{
    public class UserEntity : Entity
    {
        public int Id { get; set; }

        public string Username { get; set; }
    }
}