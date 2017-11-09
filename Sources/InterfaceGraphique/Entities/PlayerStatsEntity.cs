using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceGraphique.Entities
{
    public class PlayerStatsEntity
    {
        public int UserId { get; set; }

        public int Points { get; set; }

        public int GamesWon { get; set; }

        public int TournamentsWon { get; set; }
    }
}
