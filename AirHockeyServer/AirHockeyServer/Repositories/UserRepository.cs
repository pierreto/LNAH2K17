using AirHockeyServer.DatabaseCore;
using AirHockeyServer.Entities;
using AirHockeyServer.Mapping;
using AirHockeyServer.Pocos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace AirHockeyServer.Repositories
{
    public class UserRepository : Repository<UserRepository>
    {

        public async Task<UserEntity> GetUserById(int id)
        {
            try
            {
                IEnumerable<UserPoco> userPocoEnum = await DataProvider.GetBy<UserPoco, int>("test_users", "id_user", id);
                UserPoco userPoco = userPocoEnum.ToList().First();
                UserEntity userEntity = MapperManager.Map<UserPoco, UserEntity>(userPoco);
                return userEntity;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[UserRepository.GetUserById] " + e.ToString());
                return null;
            }
        }

        public async Task<UserEntity> GetUserByUsername(string username)
        {
            try
            {
                IEnumerable<UserPoco> userPocoEnum = await DataProvider.GetBy<UserPoco, string>("test_users", "username", username);
                UserPoco userPoco = userPocoEnum.ToList().First();
                UserEntity userEntity = MapperManager.Map<UserPoco, UserEntity>(userPoco);
                return userEntity;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[UserRepository.GetUserByUsername] " + e.ToString());
                return null;
            }
        }

        public async Task<List<UserEntity>> GetAllUsers()
        {
            try
            {
                IEnumerable<UserPoco> userPocoEnum = await DataProvider.GetAll<UserPoco>("test_users");
                List<UserPoco> userPocos = userPocoEnum.ToList();
                List<UserEntity> userEntities = MapperManager.Map<UserPoco, UserEntity>(userPocos);
                return userEntities;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[UserRepository.GetAllUsers] " + e.ToString());
                return null;
            }
        }
    }
}