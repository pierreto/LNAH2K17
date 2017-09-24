using System;

namespace AirHockeyServer.Entities
{
    public class Member : Entity
    {
        public Guid Id { get; set; }

        public string IpAdress { get; set; }
    }
}