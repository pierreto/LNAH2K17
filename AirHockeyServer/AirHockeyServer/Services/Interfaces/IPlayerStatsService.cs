using AirHockeyServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AirHockeyServer.Services.Interfaces
{
    public interface IPlayerStatsService
    {
        Task AddPoints(int userId, int pointsNb);

        Task IncrementGamesWon(int userId);

        Task IncrementTournamentsWon(int userId);

        Task<StatsEntity> GetPlayerStats(int userId);

        Task<List<AchievementEntity>> GetAchievements(int userId);

        Task SetPlayerAchievements(int userId);

        Task UpdateAchievements(int userId);
    }
}