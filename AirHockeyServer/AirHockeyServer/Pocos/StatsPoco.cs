using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Web;

namespace AirHockeyServer.Pocos
{
    [Table(Name = "players_stats")]
    public class StatsPoco
    {
        [Column(IsPrimaryKey = true, Name = "user_id")]
        public int UserId { get; set; }

        [Column(Name = "points")]
        public int Points { get; set; }

        [Column(Name = "games_won")]
        public int GamesWon { get; set; }

        [Column(Name = "tournaments_won")]
        public int TournamentsWon { get; set; }
    }
}