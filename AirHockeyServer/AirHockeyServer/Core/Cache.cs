using AirHockeyServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirHockeyServer.Core
{
    public class Cache
    {
        public static Dictionary<Guid, GameEntity> Games;
        public static Dictionary<int, TournamentEntity> Tournaments;

        public Cache()
        {
            Games = new Dictionary<Guid, GameEntity>();
            Tournaments = new Dictionary<int, TournamentEntity>();
        }
}
}