using AirHockeyServer.Entities;
using AirHockeyServer.Mapping;
using AirHockeyServer.Pocos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AirHockeyServer.Services;
using AirHockeyServer.Repositories.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace AirHockeyServer.Repositories
{
    public class PasswordRepository : Repository, IPasswordRepository
    {
        IUserRepository UserRepository;

        public PasswordRepository(MapperManager mapperManager, IUserRepository userRepository)
            : base(mapperManager)
        {
            UserRepository = userRepository;
        }

        public async Task<PasswordEntity> GetPasswordById(int id)
        {
            try
            {
                using (MyDataContext DC = new MyDataContext())
                {
                    var query =
                        from password in DC.Passwords
                        where (password.Id == id)
                        select password;
                    List<PasswordPoco> pP = await Task<List<PasswordPoco>>.Run(
                        () => query.ToList<PasswordPoco>());
                    if (pP.Any())
                    {
                        return MapperManager.Mapper.Map<PasswordPoco, PasswordEntity>(pP.First());
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (LoginException e)
            {
                System.Diagnostics.Debug.WriteLine("[PasswordRepository.GetPasswordById] " + e.ToString());
                throw e;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[PasswordRepository.GetPasswordById] " + e.ToString());
                throw e;
            }
        }

        public async Task<PasswordEntity> GetPasswordByUserId(int userId)
        {
            try
            {
                using (MyDataContext DC = new MyDataContext())
                {
                    var query =
                        from password in DC.Passwords
                        where (password.UserId == userId)
                        select password;
                    List<PasswordPoco> pP = await Task<List<PasswordPoco>>.Run(
                        () => query.ToList<PasswordPoco>());
                    if (pP.Any())
                    {
                        return MapperManager.Mapper.Map<PasswordPoco, PasswordEntity>(pP.First());
                    }
                    else
                    {
                        throw new PasswordException("Unable to get [password] by [id_user] = " + userId);
                    }
                }
            }
            catch (PasswordException e)
            {
                System.Diagnostics.Debug.WriteLine("[PasswordRepository.GetPasswordByUserId] " + e.ToString());
                throw e;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[PasswordRepository.GetPasswordByUserId] " + e.ToString());
                throw e;
            }
        }

        public async Task PostPassword(PasswordEntity passwordEntity)
        {
            try
            {
                using (MyDataContext DC = new MyDataContext())
                {
                    PasswordPoco pP = MapperManager.Mapper.Map<PasswordEntity, PasswordPoco>(passwordEntity);
                    var sha1 = new SHA1CryptoServiceProvider();
                    pP.Password =
                            Convert.ToBase64String(
                                sha1.ComputeHash(
                                    Encoding.UTF8.GetBytes(pP.Password)));
                    DC.GetTable<PasswordPoco>().InsertOnSubmit(pP);
                    await Task.Run(() => DC.SubmitChanges());
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[PasswordRepository.PostPassword] " + e.ToString());
                throw new LoginException("Unable to create password");
            }
        }
    }
}