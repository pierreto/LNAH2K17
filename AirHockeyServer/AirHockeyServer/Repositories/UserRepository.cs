using AirHockeyServer.DatabaseCore;
using AirHockeyServer.Entities;
using AirHockeyServer.Mapping;
using AirHockeyServer.Pocos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AirHockeyServer.Services;

namespace AirHockeyServer.Repositories
{
    public class UserRepository : Repository<UserRepository>
    {

        public async Task<UserEntity> GetUserById(int id)
        {
            try
            {
                IEnumerable<UserPoco> userPocoEnum = await DataProvider.GetBy<UserPoco, int>("test_users", "id_user", id);
                List<UserPoco> userPocos = userPocoEnum.ToList();
                if (userPocos.Any())
                {
                    UserPoco userPoco = userPocos.First();
                    UserEntity userEntity = MapperManager.Mapper.Map<UserPoco, UserEntity>(userPoco);
                    return userEntity;
                }
                else
                {
                    throw new UserException("Unable to get [user] by [id] = " + id);
                }

            }
            catch (UserException e)
            {
                System.Diagnostics.Debug.WriteLine("[UserRepository.GetUserById] " + e.ToString());
                throw e;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[UserRepository.GetUserById] " + e.ToString());
                throw e;
            }
        }

        public async Task<UserEntity> GetUserByUsername(string username)
        {
            try
            {
                IEnumerable<UserPoco> userPocoEnum = await DataProvider.GetBy<UserPoco, string>("test_users", "username", username);
                List<UserPoco> userPocos = userPocoEnum.ToList();
                if (userPocos.Any())
                {
                    UserPoco userPoco = userPocos.First();
                    UserEntity userEntity = MapperManager.Mapper.Map<UserPoco, UserEntity>(userPoco);
                    return userEntity;
                }
                else
                {
                    return null;
                }

            }
            catch (UserException e)
            {
                System.Diagnostics.Debug.WriteLine("[UserRepository.GetUserByUsername] " + e.ToString());
                throw e;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[UserRepository.GetUserByUsername] " + e.ToString());
                throw e;
            }
        }

        public async Task<List<UserEntity>> GetAllUsers()
        {
            try
            {
                IEnumerable<UserPoco> userPocoEnum = await DataProvider.GetAll<UserPoco>("test_users");
                List<UserPoco> userPocos = userPocoEnum.ToList();
                List<UserEntity> userEntities = MapperManager.Mapper.Map<List<UserPoco>, List<UserEntity>>(userPocos);
                return userEntities;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[UserRepository.GetAllUsers] " + e.ToString());
                throw new UserException("Unable to get all users");
            }
        }

        public void PostUser(UserEntity userEntity)
        {
            try
            {
                UserPoco uP = MapperManager.Mapper.Map<UserEntity, UserPoco>(userEntity);
                //DataProvider.Post(uP);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[UserRepository.PostUser] " + e.ToString());
                throw new UserException("Unable to create user");
            }
        }
    }
}