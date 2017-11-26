using AirHockeyServer.Core;
using AirHockeyServer.Entities;
using AirHockeyServer.Mapping;
using AirHockeyServer.Pocos;
using AirHockeyServer.Repositories.Interfaces;
using AirHockeyServer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AirHockeyServer.Repositories
{
    public class PlayerStatsRepository : Repository, IPlayerStatsRepository
    {
        private Table<StatsPoco> StatsTable;

        private Table<AchievementPoco> AchievementsTable { get; set; }

        public PlayerStatsRepository(MapperManager mapperManager)
            :base(mapperManager)
        {
        }

        public async Task<StatsEntity> CreatePlayerStats(StatsEntity playerStats)
        {
            try
            {
                using (MyDataContext DC = new MyDataContext())
                {
                    StatsPoco newStats = MapperManager.Map<StatsEntity, StatsPoco>(playerStats);
                    DC.GetTable<StatsPoco>().InsertOnSubmit(newStats);

                    await Task.Run(() => DC.SubmitChanges());

                    return await GetPlayerStat(playerStats.UserId);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[PlayerStatsRepository.CreateNewMap] " + e.ToString());
                return null;
            }
        }

        public async Task<StatsEntity> GetPlayerStat(int userId)
        {
            try
            {
                using (MyDataContext DC = new MyDataContext())
                {
                    IQueryable<StatsPoco> queryable =
                    from stats in DC.GetTable<StatsPoco>() where stats.UserId == userId select stats;

                    var results = await Task.Run(
                        () => queryable.ToArray());

                    StatsPoco result = results.Length > 0 ? results.First() : null;
                    return MapperManager.Map<StatsPoco, StatsEntity>(result);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[PlayerStatsRepository.GetPlayerStat] " + e.ToString());
                return null;
            }
        }

        public async Task<List<StatsEntity>> GetAllPlayerStats()
        {
            try
            {
                using (MyDataContext DC = new MyDataContext())
                {
                    IQueryable<StatsPoco> queryable =
                    from stats in DC.GetTable<StatsPoco>() select stats;

                    var results = await Task.Run(
                        () => queryable.ToList());
                    
                    return MapperManager.Map<List<StatsPoco>, List<StatsEntity>>(results);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[PlayerStatsRepository.GetPlayerStat] " + e.ToString());
                return null;
            }
        }

        public async Task UpdatePlayerStats(int userId, StatsEntity updatedPlayerStats)
        {
            try
            {
                using (MyDataContext DC = new MyDataContext())
                {
                    StatsPoco _updatedMap = MapperManager.Map<StatsEntity, StatsPoco>(updatedPlayerStats);
                    var query =
                        from stats in DC.GetTable<StatsPoco>() where stats.UserId == updatedPlayerStats.UserId select stats;

                    var results = query.ToArray();
                    var existingStats = results.First();

                    existingStats.Points = updatedPlayerStats.Points;
                    existingStats.GamesWon = updatedPlayerStats.GamesWon;
                    existingStats.TournamentsWon = updatedPlayerStats.TournamentsWon;

                    await Task.Run(() => DC.SubmitChanges());
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[PlayerStatsRepository.UpdatePlayerStats] " + e.ToString());
            }
        }

        public async Task CreateAllAchievements(int userId)
        {
            try
            {
                using (MyDataContext DC = new MyDataContext())
                {
                    List<AchievementPoco> aPs = new List<AchievementPoco>();
                    foreach (AchivementType achievement in Enum.GetValues(typeof(AchivementType)))
                    {
                        AchievementPoco achievementPoco = new AchievementPoco
                        {
                            AchievementType = achievement.ToString(),
                            IsEnabled = false,
                            UserId = userId
                        };
                        aPs.Add(achievementPoco);
                    }
                    foreach(var aP in aPs)
                    {
                        DC.GetTable<AchievementPoco>().InsertOnSubmit(aP);
                    }

                    await Task.Run(() => DC.SubmitChanges());
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[PlayerStatsRepository.CreateAchievement] " + e.ToString());
            }
        }

        public async Task<List<AchievementEntity>> GetAchievements(int userId)
        {
            try
            {
                using (MyDataContext DC = new MyDataContext())
                {
                    IQueryable<AchievementPoco> queryable =
                    from achievements in DC.GetTable<AchievementPoco>() where achievements.UserId == userId select achievements;

                    var results = await Task.Run(
                        () => queryable.ToArray());
                    
                    return GetAchievementEntitiesFromPocos(results.ToList());
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[PlayerStatsRepository.GetAchievements] " + e.ToString());
                return null;
            }
        }

        public async Task UpdateAchievement(int userId, AchivementType achivementType, bool isEnabled)
        {
            try
            {
                using (MyDataContext DC = new MyDataContext())
                {
                    var query =
                    from achievements in DC.GetTable<AchievementPoco>() where achievements.UserId == userId && achivementType.ToString() == achievements.AchievementType select achievements;

                    var results = query.ToArray();
                    var existingAchievement = results.First();

                    existingAchievement.IsEnabled = isEnabled;

                    await Task.Run(() => DC.SubmitChanges());
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[PlayerStatsRepository.UpdateAchievement] " + e.ToString());
            }
        }

        public async Task UpdateAchievements(int userId, List<AchivementType> achievementTypes)
        {
            try
            {
                List<string> stringTypes = new List<string>();
                achievementTypes.ForEach(x => stringTypes.Add(x.ToString()));

                using (MyDataContext DC = new MyDataContext())
                {
                    var pocos = DC.GetTable<AchievementPoco>()
                        .Where(x => userId == x.UserId &&
                            stringTypes.Contains(x.AchievementType))
                        .ToList();

                    pocos.ForEach(x => x.IsEnabled = true);

                    await Task.Run(() => DC.SubmitChanges());
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[PlayerStatsRepository.UpdateAchievements] " + e.ToString());
            }
        }

        public async Task CreateAchievements(int userId, List<AchivementType> achievements)
        {
            try
            {
                using (MyDataContext DC = new MyDataContext())
                {
                    List<AchievementPoco> pocos = new List<AchievementPoco>();
                    foreach (AchivementType achievement in achievements)
                    {
                        AchievementPoco achievementPoco = new AchievementPoco
                        {
                            AchievementType = achievement.ToString(),
                            IsEnabled = true,
                            UserId = userId
                        };
                        pocos.Add(achievementPoco);
                    }
                    foreach (var poco in pocos)
                    {
                        DC.GetTable<AchievementPoco>().InsertOnSubmit(poco);
                    }

                    await Task.Run(() => DC.SubmitChanges());
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[PlayerStatsRepository.CreateAchievements] " + e.ToString());
            }
        }

        private List<AchievementEntity> GetAchievementEntitiesFromPocos(List<AchievementPoco> pocos)
        {
            List<AchievementEntity> results = new List<AchievementEntity>();
            foreach (var poco in pocos)
            {
                results.Add(new AchievementEntity(poco));
            }

            return results;
        }

    }
}