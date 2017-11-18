using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceGraphique.Entities
{
    public class PlayerEndOfGameStatsEntity
    {
        public int Id { get; set; }

        public int PointsWon { get; set; }

        public List<Achievement> UnlockedAchievements { get; set; }
        
    }
}
