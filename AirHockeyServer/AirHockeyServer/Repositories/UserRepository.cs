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

    //public async Task<UserEntity> GetUserById(int id)
    public UserEntity GetUserById(int id)
        {
            try
            {
                UserPoco userPoco = DataProvider.GetById<UserPoco>("test_users", id);
                UserEntity userEntity = MapperManager.Map<UserPoco, UserEntity>(userPoco);
                return userEntity;
            }
            catch (Exception w)
            {
                return null;
            }
            
        }

        public async Task<List<UserEntity>> GetAllUsers()
        {
            try
            {
                var userPocos = DataProvider.GetAll<UserPoco>("test_users");
                List<UserEntity> userEntities = MapperManager.Map<UserPoco,UserEntity>(userPocos);
                return userEntities;
            }
            catch
            {
                return null;
            }
        }
    }
}