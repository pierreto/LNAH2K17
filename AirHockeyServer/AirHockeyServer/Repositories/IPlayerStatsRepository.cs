using AirHockeyServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AirHockeyServer.Repositories
{
    public interface IPlayerStatsRepository
    {
        Task<StatsEntity> GetPlayerStat(int userId);

        Task UpdatePlayerStats(int userId, StatsEntity playerStats);
        
        Task<StatsEntity> CreatePlayerStats(StatsEntity playerStats);
    }
}