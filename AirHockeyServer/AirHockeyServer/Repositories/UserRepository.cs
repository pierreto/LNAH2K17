using AirHockeyServer.DatabaseCore;
using AirHockeyServer.Entities;
using AirHockeyServer.Mapping;
using AirHockeyServer.Pocos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AirHockeyServer.Repositories
{
    public class UserRepository : Repository<UserRepository>
    {

    public async Task<UserEntity> GetUserById(int id)
        {
            try
            {
                UserPoco userPoco = await DataProvider.GetById<UserPoco>("test_users", id);
                UserEntity userEntity = MapperManager.Map<UserPoco, UserEntity>(userPoco);
                return userEntity;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[UserRepository.GetUserById] " + e.ToString());
                return null;
            }
        }

        public async Task<List<UserEntity>> GetAllUsers()
        {
            try
            {
                var userPocos = await DataProvider.GetAll<UserPoco>("test_users");
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