using System;

namespace AirHockeyServer.Entities
{
    public class RankingEntity : Entity
    {
        public string Username { get; set; }

        public int GamesWon { get; set; }

        public int TournamentsWon { get; set; }

        public int Points { get; set; }
    }
}