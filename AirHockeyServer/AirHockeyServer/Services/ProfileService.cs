using System.Threading.Tasks;
using AirHockeyServer.Entities;
using AirHockeyServer.Services.Interfaces;
using AirHockeyServer.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace AirHockeyServer.Services
{
    public class ProfileService : IProfileService
    {

        private IUserRepository UserRepository;
        private IPlayerStatsRepository PlayerStatsRepository;
        private IGameRepository GameRepository;
        private ITournamentRepository TournamentRepository;

        public ProfileService(IUserRepository userRepository, IPlayerStatsRepository playerStatsRepository, IGameRepository gameRepository, ITournamentRepository tournamentRepository)
        {
            UserRepository = userRepository;
            PlayerStatsRepository = playerStatsRepository;
            GameRepository = gameRepository;
            TournamentRepository = tournamentRepository;
        }

        public async Task<ProfileEntity> GetProfileById(int id)
        {
            UserEntity uE = await UserRepository.GetUserById(id);
            StatsEntity sE = await PlayerStatsRepository.GetPlayerStat(id);
            AchievementEntity[] aEL = (await PlayerStatsRepository.GetAchievements(id)).OrderBy(x=> x.Category).ThenBy(x => x.Order).ToArray();
            int gamesPlayed = await GameRepository.GetUserGamesNb(id);
            int tournamentsPlayed = await TournamentRepository.GetUserTournamentsNb(id);
            ProfileEntity pE = new ProfileEntity
            {
                UserEntity = uE,
                StatsEntity = sE,
                AchievementEntities = aEL,
                GamesPlayed = gamesPlayed,
                TournamentsPlayed = tournamentsPlayed
            };
            return pE;
        }
    }
}