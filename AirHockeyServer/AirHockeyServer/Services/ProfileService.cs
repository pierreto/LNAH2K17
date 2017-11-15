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

        public ProfileService(IUserRepository userRepository, IPlayerStatsRepository playerStatsRepository)
        {
            UserRepository = userRepository;
            PlayerStatsRepository = playerStatsRepository;
        }

        public async Task<ProfileEntity> GetProfileById(int id)
        {
            UserEntity uE = await UserRepository.GetUserById(id);
            StatsEntity sE = await PlayerStatsRepository.GetPlayerStat(id);
            AchievementEntity[] aEL = (await PlayerStatsRepository.GetAchievements(id)).OrderBy(x=> x.Category).ThenBy(x => x.Order).ToArray();
            ProfileEntity pE = new ProfileEntity
            {
                UserEntity = uE,
                StatsEntity = sE,
                AchievementEntities = aEL
            };
            return pE;
        }

        public async Task<ProfileEntity> GetProfileByUsername(string username)
        {
            UserEntity uE = await UserRepository.GetUserByUsername(username);
            StatsEntity sE = await PlayerStatsRepository.GetPlayerStat(uE.Id);
            AchievementEntity[] aEL = (await PlayerStatsRepository.GetAchievements(uE.Id)).OrderBy(x => x.Category).ThenBy(x => x.Order).ToArray();
            ProfileEntity pE = new ProfileEntity
            {
                UserEntity = uE,
                StatsEntity = sE,
                AchievementEntities = aEL
            };
            return pE;
        }
    }
}