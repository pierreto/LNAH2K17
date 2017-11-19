using System;

namespace AirHockeyServer.Entities
{
    public class UserEntity : Entity
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Created { get; set; }

        public string Profile { get; set; }
    
    }
}