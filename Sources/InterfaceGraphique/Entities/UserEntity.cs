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

        // OU STRING? je sais pas =(
        public DateTime Date { get; set; }

        public string Profile { get; set; }
    }
}
