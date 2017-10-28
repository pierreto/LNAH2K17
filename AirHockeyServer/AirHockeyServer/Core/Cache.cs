using AirHockeyServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirHockeyServer.Core
{
    public class Cache
    {
        public static Dictionary<int, GameEntity> Games;

        public static Dictionary<int, TournamentEntity> Tournaments;

        public Cache()
        {
            Games = new Dictionary<int, GameEntity>();
            Tournaments = new Dictionary<int, TournamentEntity>();
        }
    }
}