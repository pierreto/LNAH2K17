using AirHockeyServer.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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

            LoadStoreItems();
        }

        private void LoadStoreItems()
        {
            string path = System.Web.Hosting.HostingEnvironment.MapPath("/") + "\\..\\..\\Exe\\données\\StoreItems.json";
            using (StreamReader reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd();
                List<StoreItemEntity> items = JsonConvert.DeserializeObject<List<StoreItemEntity>>(json);

                items.ForEach(item => StoreItems.Add(item.Id, item));
            }
        }
    }
}