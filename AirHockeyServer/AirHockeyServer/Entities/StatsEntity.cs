using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirHockeyServer.Entities
{
    public class StatsEntity
    {
        public int UserId { get; set; }

        public int Points { get; set; }

        public int GamesWon { get; set; }

        public int TournamentsWon { get; set; }
    }
}