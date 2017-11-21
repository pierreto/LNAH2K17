using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceGraphique.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Date { get; set; }

        public string Profile { get; set; }

        public bool IsSelected { get; set; }

        public bool AlreadyPlayedGame { get; set; }

        public bool AlreadyUsedFatEditor { get; set; }

        public bool AlreadyUsedLightEditor { get; set; }
    }
}
