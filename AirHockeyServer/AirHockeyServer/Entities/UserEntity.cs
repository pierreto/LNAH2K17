using System;

namespace AirHockeyServer.Entities
{
    public class UserEntity : Entity
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
    }
}