using System;

namespace AirHockeyServer.Entities
{
    public class ProfileEntity : Entity
    {
        public UserEntity UserEntity { get; set; }
        public StatsEntity StatsEntity { get; set; }
        public AchievementEntity[] AchievementEntities { get; set; }
        public int GamesPlayed { get; set; }
        public int TournamentsPlayed { get; set; }

        public ProfileEntity()
        {
        }
    }
}