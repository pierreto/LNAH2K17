using AirHockeyServer.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;


namespace AirHockeyServer.Core
{
    public class Cache
    {
        public static Dictionary<Guid, GameEntity> Games;

        public static Dictionary<int, TournamentEntity> Tournaments;

        public static Dictionary<int, StoreItemEntity> StoreItems;

        public static Dictionary<AchivementType, AchievementEntity> Achievements;

        static Mutex playersMutex;
        public static List<UserEntity> PlayingPlayers;

        public static List<UserEntity> GetPlayers()
        {
            return PlayingPlayers;
        }

        public static void AddPlayer(UserEntity user)
        {
            playersMutex.WaitOne();
            if(!PlayingPlayers.Exists(x => user.Id == x.Id))
            {
                PlayingPlayers.Add(user);
            }
            playersMutex.ReleaseMutex();
        }

        public Cache()
        {
            Games = new Dictionary<Guid, GameEntity>();
            Tournaments = new Dictionary<int, TournamentEntity>();
            StoreItems = new Dictionary<int, StoreItemEntity>();
            PlayingPlayers = new List<UserEntity>();
            Achievements = new Dictionary<AchivementType, AchievementEntity>();
            playersMutex = new Mutex();

            LoadStoreItems();
            LoadAchievements();
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

        private void LoadAchievements()
        {
            string path = System.Web.Hosting.HostingEnvironment.MapPath("/") + "\\..\\..\\Exe\\données\\Achievements.json";
            using (StreamReader reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd();
                List<AchievementEntity> items = JsonConvert.DeserializeObject<List<AchievementEntity>>(json);

                items.ForEach(item => Achievements.Add(item.AchivementType, item));
            }
        }

        public static void RemovePlayer(UserEntity user)
        {
            playersMutex.WaitOne();
            UserEntity removedPlayer = PlayingPlayers.Find(x => x.Id == user.Id);
            if (removedPlayer != null)
            {
                PlayingPlayers.Remove(removedPlayer);
            }
            playersMutex.ReleaseMutex();
        }

        internal static void RemovePlayer(int userId)
        {
            playersMutex.WaitOne();

            UserEntity removedPlayer = PlayingPlayers.Find(x => x.Id == userId);
            if (removedPlayer != null)
            {
                PlayingPlayers.Remove(removedPlayer);
            }
            playersMutex.ReleaseMutex();
        }
    }
}