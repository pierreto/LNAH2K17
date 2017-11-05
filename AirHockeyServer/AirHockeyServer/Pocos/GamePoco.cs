using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Web;

namespace AirHockeyServer.Pocos
{
    [Table(Name = "games")]
    public class GamePoco
    {
        [Column(IsPrimaryKey = true, Name = "id")]
        public string Id { get; set; }

        [Column(Name = "winner")]
        public int Winner { get; set; }

        [Column(Name = "map_played")]
        public int PlayedMap { get; set; }
    }
}