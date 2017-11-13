using AirHockeyServer.Entities;
using AirHockeyServer.Mapping;
using AirHockeyServer.Pocos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AirHockeyServer.Services;
using AirHockeyServer.Repositories.Interfaces;

namespace AirHockeyServer.Repositories
{
    public class UserRepository : Repository, IUserRepository
    {

        public UserRepository(MapperManager mapperManager)
            : base(mapperManager)
        {

        }

        public async Task<UserEntity> GetUserById(int id)
        {
            try
            {
                using (MyDataContext DC = new MyDataContext())
                {
                    var query =
                        from user in DC.UsersTable
                        where (user.Id == id)
                        select user;
                    List<UserPoco> uP = await Task.Run(
                        () => query.ToList<UserPoco>());
                    if (uP.Any())
                    {
                        return MapperManager.Mapper.Map<UserPoco, UserEntity>(uP.First());
                    }
                    else
                    {
                        throw new UserException("Unable to get [user] by [id] = " + id);
                    }
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
                using (MyDataContext DC = new MyDataContext())
                {
                    var query =
                        from user in DC.UsersTable
                        where (user.Username == username)
                        select user;
                    List<UserPoco> uP = await Task<List<UserPoco>>.Run(
                        () => query.ToList<UserPoco>());
                    if(uP.Any())
                    {
                        return MapperManager.Mapper.Map<UserPoco, UserEntity>(uP.First());
                    } else
                    {
                        return null;
                    }
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
                using(MyDataContext DC = new MyDataContext())
                {
                    var query =
                        from user in DC.UsersTable
                        select user;

                    List<UserPoco> userPocos = await Task.Run(
                        () => query.ToList<UserPoco>());

                    return MapperManager.Mapper.Map<List<UserPoco>, List<UserEntity>>(userPocos);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[UserRepository.GetAllUsers] " + e.ToString());
                throw new UserException("Unable to get all users");
            }
        }

        public async Task PostUser(UserEntity userEntity)
        {
            try
            {
                using (MyDataContext DC = new MyDataContext())
                {
                    UserPoco uP = MapperManager.Mapper.Map<UserEntity, UserPoco>(userEntity);
                    DC.GetTable<UserPoco>().InsertOnSubmit(uP);
                    await Task.Run(() => DC.SubmitChanges());
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[UserRepository.PostUser] " + e.ToString());
                throw new UserException("Unable to create user");
            }
        }
    }
}