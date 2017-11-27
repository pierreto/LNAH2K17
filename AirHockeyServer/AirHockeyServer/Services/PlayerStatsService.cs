using AirHockeyServer.Core;
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

        public async Task CreateAllAchievements(int userId)
        {
            await PlayerStatsRepository.CreateAllAchievements(userId);
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

        public async Task<List<AchievementEntity>> GetAchievementsToUpdate(int userId)
        {
            List<AchievementEntity> achievements = await GetAchievements(userId);

            var stats = await GetPlayerStats(userId);
            if(stats == null)
            {
                stats = new StatsEntity();
            }

            List<AchievementEntity> typesAdded = new List<AchievementEntity>();

            AchievementUpdateNeeded(stats.Points, AchivementType.FivePoints, 5, achievements, typesAdded);
            AchievementUpdateNeeded(stats.Points, AchivementType.ThirtyPoints, 30, achievements, typesAdded);
            AchievementUpdateNeeded(stats.Points, AchivementType.EightyPoints, 80, achievements, typesAdded);

            AchievementUpdateNeeded(stats.GamesWon, AchivementType.FirstGameWon, 1, achievements, typesAdded);
            AchievementUpdateNeeded(stats.GamesWon, AchivementType.FiveGameWon, 5, achievements, typesAdded);
            AchievementUpdateNeeded(stats.GamesWon, AchivementType.TenGameWon, 10, achievements, typesAdded);

            AchievementUpdateNeeded(stats.TournamentsWon, AchivementType.FirstTournamentWon, 1, achievements, typesAdded);
            AchievementUpdateNeeded(stats.TournamentsWon, AchivementType.FiveTournamentWon, 5, achievements, typesAdded);
            AchievementUpdateNeeded(stats.TournamentsWon, AchivementType.TenTournamentWon, 10, achievements, typesAdded);

            int tournamentsNb = await TournamentRepository.GetUserTournamentsNb(userId);
            int gameNb = await GameRepository.GetUserGamesNb(userId);

            AchievementUpdateNeeded(tournamentsNb, AchivementType.FirstTournamentPlayed, 1, achievements, typesAdded);
            AchievementUpdateNeeded(tournamentsNb, AchivementType.FiveTournamentsPlayed, 5, achievements, typesAdded);
            AchievementUpdateNeeded(tournamentsNb, AchivementType.TenTournamentsPlayed, 10, achievements, typesAdded);

            AchievementUpdateNeeded(gameNb, AchivementType.FirstGamePlayed, 1, achievements, typesAdded);
            AchievementUpdateNeeded(gameNb, AchivementType.FiveGamesPlayed, 5, achievements, typesAdded);
            AchievementUpdateNeeded(gameNb, AchivementType.TenGamesPlayed, 10, achievements, typesAdded);

            return typesAdded;

        }


        public async Task UpdateAchievements(int userId, List<AchivementType> achievements)
        {
            await this.PlayerStatsRepository.UpdateAchievements(userId, achievements);
        }

        private void AchievementUpdateNeeded(int baseVal, 
            AchivementType achivementType, 
            int minVal, 
            List<AchievementEntity> userAchievements,
            List<AchievementEntity> achievementAdded)
        {
            var achivement = userAchievements.Find(x => x.AchivementType == achivementType);
            if ((achivement == null && baseVal >= minVal) || (baseVal >= minVal && achivement != null && !achivement.IsEnabled))
            {
                achievementAdded.Add(Cache.Achievements[achivementType]);
            }
        }

        public List<AchievementEntity> GetAchievements()
        {
            return Cache.Achievements.Values.ToList();
        }

        public async Task CreateAchievements(int userId, List<AchivementType> achievements)
        {
            await this.PlayerStatsRepository.CreateAchievements(userId, achievements);
        }
    }
}