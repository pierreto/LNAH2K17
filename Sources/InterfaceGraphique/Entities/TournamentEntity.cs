using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceGraphique.Entities
{
    public class TournamentEntity
    {
        public int Id { get; set; }

        public List<UserEntity> Players { get; set; }

        public List<GameEntity> Games { get; set; }

        public MapEntity SelectedMap { get; set; }
    }
}
