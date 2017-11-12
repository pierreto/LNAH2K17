using AirHockeyServer.Entities;
using AirHockeyServer.Repositories;
using AirHockeyServer.Repositories.Interfaces;
using AirHockeyServer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AirHockeyServer.Services
{
    public class PlayerStatsService : IPlayerStatsService
    {
        protected IPlayerStatsRepository PlayerStatsRepository { get; set; }
        public ITournamentRepository TournamentRepository { get; set; }
        public IGameRepository GameRepository { get; set; }

        public PlayerStatsService(IPlayerStatsRepository playerStatsRepository,
            ITournamentRepository tournamentRepository,
            IGameRepository gameRepository)
        {
            PlayerStatsRepository = playerStatsRepository;
            TournamentRepository = tournamentRepository;
            GameRepository = gameRepository;
        }

        public async Task SetPlayerAchievements(int userId)
        {
            await PlayerStatsRepository.CreateAchievement(userId);
        }

        public async Task AddPoints(int userId, int pointsNb)
        {
            var stats = await GetPlayerStats(userId);

            if (stats == null)
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

        public async Task<List<AchievementEntity>> GetAchievements(int userId)
        {
            return await PlayerStatsRepository.GetAchievements(userId);
        }

        public async Task UpdateAchievements(int userId)
        {
            List<AchievementEntity> achievements = await GetAchievements(userId);

            if(achievements.Count == 0)
            {
                await SetPlayerAchievements(userId);
                achievements = await GetAchievements(userId);
            }

            var stats = await GetPlayerStats(userId);
            
            await UpdateAchievement(stats.Points, AchivementType.FivePoints, 5, userId, achievements);
            await UpdateAchievement(stats.Points, AchivementType.ThirtyPoints, 30, userId, achievements);
            await UpdateAchievement(stats.Points, AchivementType.EightyPoints, 80, userId, achievements);

            await UpdateAchievement(stats.GamesWon, AchivementType.FirstGameWon, 1, userId, achievements);
            await UpdateAchievement(stats.GamesWon, AchivementType.FiveGameWon, 5, userId, achievements);
            await UpdateAchievement(stats.GamesWon, AchivementType.TenGameWon, 10, userId, achievements);

            await UpdateAchievement(stats.TournamentsWon, AchivementType.FirstTournamentWon, 1, userId, achievements);
            await UpdateAchievement(stats.TournamentsWon, AchivementType.FiveTournamentWon, 5, userId, achievements);
            await UpdateAchievement(stats.TournamentsWon, AchivementType.TenTournamentWon, 10, userId, achievements);

            int tournamentsNb = await TournamentRepository.GetUserTournamentsNb(userId);
            int gameNb = await GameRepository.GetUserGamesNb(userId);

            await UpdateAchievement(tournamentsNb, AchivementType.FirstTournamentPlayed, 1, userId, achievements);
            await UpdateAchievement(tournamentsNb, AchivementType.FiveTournamentsPlayed, 5, userId, achievements);
            await UpdateAchievement(tournamentsNb, AchivementType.TenTournamentPlayed, 10, userId, achievements);

            await UpdateAchievement(gameNb, AchivementType.FirstGamePlayed, 1, userId, achievements);
            await UpdateAchievement(gameNb, AchivementType.FiveGamesPlayed, 5, userId, achievements);
            await UpdateAchievement(gameNb, AchivementType.TenGamesPlayed, 10, userId, achievements);

        }

        private async Task UpdateAchievement(int baseVal, 
            AchivementType achivementType, 
            int minVal, 
            int userId,
            List<AchievementEntity> achievements)
        {
            var achivement = achievements.Find(x => x.AchivementType == achivementType);
            if (achivement != null && baseVal >= minVal && !achivement.IsEnabled)
            {
                await this.PlayerStatsRepository.UpdateAchievement(userId, achivementType, true);
            }
        }
    }
}