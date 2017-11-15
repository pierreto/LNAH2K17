using AirHockeyServer.Entities;
using AirHockeyServer.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AirHockeyServer.Services.Interfaces;

namespace AirHockeyServer.Services
{
    public class RankingService : IRankingService
    {
        private IUserRepository UserRepository;
        private IPlayerStatsRepository PlayerStatsRepository;

        public RankingService(IUserRepository userRepository, IPlayerStatsRepository playerStatsRepository)
        {
            UserRepository = userRepository;
            PlayerStatsRepository = playerStatsRepository;
        }

        public async Task<List<RankingEntity>> GetAllRankings()
        {
            List<StatsEntity> sEL = await PlayerStatsRepository.GetAllPlayerStats();
            List<UserEntity> uEL = await UserRepository.GetAllUsers();
            List<RankingEntity> pEL = new List<RankingEntity>();
            // For every user, get his stats if he has any, and join then into a RankingEntity
            // If the user does not has a stats(user created before adding stats to db), set them to 0
            var query = from uE in uEL
                        join sE in sEL
                        on uE.Id equals sE.UserId
                        into uEsE
                        from sE in uEsE.DefaultIfEmpty()
                        select new RankingEntity
                        {
                            Id = uE.Id,
                            Username = uE.Username,
                            GamesWon = sE != null ? sE.GamesWon : 0,
                            TournamentsWon = sE != null ? sE.TournamentsWon : 0,
                            Points = sE != null ? sE.Points : 0
                      };
            List<RankingEntity> results = await Task.Run(
                        () => query.ToList<RankingEntity>());
           return results;
        }
    }
}