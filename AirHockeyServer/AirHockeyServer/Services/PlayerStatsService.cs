using AirHockeyServer.Entities;
using AirHockeyServer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AirHockeyServer.Services
{
    public class PlayerStatsService : IPlayerStatsService
    {
        public PlayerStatsService()
        {
            PlayerStatsRepository = new PlayerStatsRepository();
        }

        public IPlayerStatsRepository PlayerStatsRepository { get; }

        public async Task AddPoints(int userId, int pointsNb)
        {
            var stats = await GetPlayerStats(userId);

            if(stats == null)
            {
                stats = await CreateStats(userId);
            }
            
            stats.Points += pointsNb;
            await PlayerStatsRepository.UpdatePlayerStats(userId, stats);
        }

        public async Task IncrementGamesWon(int userId)
        {
            var stats = await GetPlayerStats(userId);

            if (stats == null)
            {
                stats = await CreateStats(userId);
            }

            stats.GamesWon += 1;
            await PlayerStatsRepository.UpdatePlayerStats(userId, stats);
        }

        public async Task IncrementTournamentsWon(int userId)
        {
            var stats = await GetPlayerStats(userId);

            if (stats == null)
            {
                stats = await CreateStats(userId);
            }

            stats.TournamentsWon += 1;
            await PlayerStatsRepository.UpdatePlayerStats(userId, stats);
        }

        public async Task<StatsEntity> GetPlayerStats(int userId)
        {
            return await PlayerStatsRepository.GetPlayerStat(userId);
        }

        private async Task<StatsEntity> CreateStats(int userId)
        {
            StatsEntity entity = new StatsEntity
            {
                UserId = userId
            };

            return await PlayerStatsRepository.CreatePlayerStats(entity);
        }
    }
}