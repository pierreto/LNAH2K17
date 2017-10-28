using AirHockeyServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AirHockeyServer.Services
{
    public interface IPlayerStatsService
    {
        Task AddPoints(int userId, int pointsNb);

        Task IncrementGamesWon(int userId);

        Task IncrementTournamentsWon(int userId);

        Task<StatsEntity> GetPlayerStats(int userId);
    }
}