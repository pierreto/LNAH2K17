using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Web;

namespace AirHockeyServer.Pocos
{
     [Table(Name="tournaments")]
    public class TournamentPoco
    {
        [Column(IsPrimaryKey = true, Name = "id")]
        public int Id { get; set; }

        [Column(Name = "winner")]
        public int Winner { get; set; }

        [Column(Name = "map_played")]
        public int PlayedMap { get; set; }

        [Column(Name = "player_1")]
        public int Player1 { get; set; }

        [Column(Name = "player_2")]
        public int Player2 { get; set; }

        [Column(Name = "player_3")]
        public int Player3 { get; set; }

        [Column(Name = "player_4")]
        public int Player4 { get; set; }
    }
}