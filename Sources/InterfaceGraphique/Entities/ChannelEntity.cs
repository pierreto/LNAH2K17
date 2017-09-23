using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceGraphique.Entities
{
    public class ChannelEntity
    {
        public Guid Id { get; set; }
        public List<MemberEntity> Members { get; set; }

        public string Name { get; set; }

        public ChannelEntity()
        {
            this.Members = new List<MemberEntity>();
        }
    }
}
