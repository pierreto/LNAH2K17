using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirHockeyServer.Entities
{
    public class ChannelEntity : Entity
    {
        public Guid Id { get; set; }
        public List<UserEntity> Members { get; set; }

        public string Name { get; set; }

        public ChannelEntity()
        {
            this.Members = new List<UserEntity>();
        }
    }
}