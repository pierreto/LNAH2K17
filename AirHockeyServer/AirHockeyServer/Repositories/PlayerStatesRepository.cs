using AirHockeyServer.DatabaseCore;
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
        public IAchievementInfoService AchievementInfoService { get; set; }

        public PlayerStatsRepository(DataProvider dataProvider, MapperManager mapperManager, IAchievementInfoService achievementInfoService)
            :base(dataProvider, mapperManager)
        {
            this.StatsTable = DataProvider.DC.GetTable<StatsPoco>();
            this.AchievementsTable = DataProvider.DC.GetTable<AchievementPoco>();
            AchievementInfoService = achievementInfoService;
        }

        public async Task<StatsEntity> CreatePlayerStats(StatsEntity playerStats)
        {
            try
            {
                StatsPoco newStats = MapperManager.Map<StatsEntity, StatsPoco>(playerStats);
                this.StatsTable.InsertOnSubmit(newStats);

                await Task.Run(() => this.DataProvider.DC.SubmitChanges());

                return await GetPlayerStat(playerStats.UserId);
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
                IQueryable<StatsPoco> queryable =
                    from stats in this.StatsTable where stats.UserId == userId select stats;

                var results = await Task<IEnumerable<StatsPoco>>.Run(
                    () => queryable.ToArray());

                StatsPoco result = results.Length > 0 ? results.First() : null;
                return MapperManager.Map<StatsPoco, StatsEntity>(result);
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
                StatsPoco _updatedMap = MapperManager.Map<StatsEntity, StatsPoco>(updatedPlayerStats);
                var query =
                    from stats in this.StatsTable where stats.UserId == updatedPlayerStats.UserId select stats;

                var results = query.ToArray();
                var existingStats = results.First();

                existingStats.Points = updatedPlayerStats.Points;
                existingStats.GamesWon = updatedPlayerStats.GamesWon;
                existingStats.TournamentsWon = updatedPlayerStats.TournamentsWon;

                await Task.Run(() => this.DataProvider.DC.SubmitChanges());
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[PlayerStatsRepository.UpdatePlayerStats] " + e.ToString());
            }
        }

        public async Task CreateAchievement(int userId, AchivementType achivementType)
        {
            try
            {
                AchievementPoco achievementPoco = new AchievementPoco
                {
                    AchievementType = achivementType.ToString(),
                    IsEnabled = false,
                    UserId = userId
                };

                this.AchievementsTable.InsertOnSubmit(achievementPoco);

                await Task.Run(() => this.DataProvider.DC.SubmitChanges());
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
                IQueryable<AchievementPoco> queryable =
                    from achievements in this.AchievementsTable where achievements.UserId == userId select achievements;

                var results = await Task<IEnumerable<AchievementPoco>>.Run(
                    () => queryable.ToArray());
               
                List<AchievementEntity> resultEntities = new List<AchievementEntity>();
                foreach (AchievementPoco poco in results)
                {
                    AchivementType achievementType = (AchivementType)Enum.Parse(typeof(AchivementType), poco.AchievementType);
                    resultEntities.Add(new AchievementEntity
                    {
                        AchivementType = achievementType,
                        EnabledImageUrl = AchievementInfoService.GetEnabledImage(achievementType),
                        DisabledImageUrl = AchievementInfoService.GetDisabledImage(achievementType),
                        IsEnabled = poco.IsEnabled,
                        Name = AchievementInfoService.GetName(achievementType),
                        Category = AchievementInfoService.GetCategory(achievementType),
                        Order = AchievementInfoService.GetOrder(achievementType)
                    });
                }


                return resultEntities;
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
                var query =
                    from achievements in this.AchievementsTable where achievements.UserId == userId && achivementType.ToString() == achievements.AchievementType select achievements;

                var results = query.ToArray();
                var existingAchievement = results.First();

                existingAchievement.IsEnabled = isEnabled;

                await Task.Run(() => this.DataProvider.DC.SubmitChanges());
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[PlayerStatsRepository.UpdateAchievement] " + e.ToString());
            }
        }

    }
}