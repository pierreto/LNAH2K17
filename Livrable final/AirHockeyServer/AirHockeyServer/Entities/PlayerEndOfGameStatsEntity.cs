using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirHockeyServer.Entities
{
    public class PlayerEndOfGameStatsEntity
    {
        public int Id { get; set; }

        public int PointsWon { get; set; }

        public List<AchievementEntity> UnlockedAchievements { get; set; }
    }
}