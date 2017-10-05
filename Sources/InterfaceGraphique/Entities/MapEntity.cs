using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceGraphique.Entities
{
    class MapEntity
    {
        public UserEntity Creator { get; set; }
        public string MapName { get; set; }
        public DateTime LastBackup { get; set; }
        public string Json { get; set; }
    }
}