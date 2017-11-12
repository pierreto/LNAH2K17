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

        public static Dictionary<int, StoreItemEntity> StoreItems;

        public Cache()
        {
            Games = new Dictionary<Guid, GameEntity>();
            Tournaments = new Dictionary<int, TournamentEntity>();
            StoreItems = new Dictionary<int, StoreItemEntity>();

            StoreItems.Add(1, new StoreItemEntity
            {
                Name = "Name1",
                Description = "Description1",
                Id = 1,
                Price = 5,
                IsGameEnabled = false
            });

            StoreItems.Add(2, new StoreItemEntity
            {
                Name = "Name2",
                Description = "Description2",
                Id = 2,
                Price = 5,
                IsGameEnabled = false
            });

            StoreItems.Add(3, new StoreItemEntity
            {
                Name = "Name3",
                Description = "Description3",
                Id = 3,
                Price = 5,
                IsGameEnabled = false
            });
        }
}
}