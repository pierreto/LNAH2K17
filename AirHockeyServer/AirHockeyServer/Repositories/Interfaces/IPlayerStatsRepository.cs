using AirHockeyServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AirHockeyServer.Repositories.Interfaces
{
    public interface IPlayerStatsRepository
    {
        Task<StatsEntity> CreatePlayerStats(StatsEntity playerStats);

        Task<StatsEntity> GetPlayerStat(int userId);

        Task UpdatePlayerStats(int userId, StatsEntity updatedPlayerStats);

        Task<List<AchievementEntity>> GetAchievements(int userId);

        Task CreateAchievement(int userId);

        Task UpdateAchievement(int userId, AchivementType achivementType, bool isEnabled);
    }
}