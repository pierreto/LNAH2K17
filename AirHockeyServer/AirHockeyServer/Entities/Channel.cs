using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirHockeyServer.Entities
{
    public class Channel : Entity
    {
        public Guid Id { get; set; }
        public List<Member> Members { get; set; }

        public string Name { get; set; }

        public Channel()
        {
            this.Members = new List<Member>();
        }
    }
}