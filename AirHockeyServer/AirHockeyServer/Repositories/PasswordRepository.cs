using AirHockeyServer.DatabaseCore;
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
    public class PasswordRepository : Repository, IPasswordRepository
    {
        IUserRepository UserRepository;

        public PasswordRepository(DataProvider dataProvider, MapperManager mapperManager, IUserRepository userRepository)
            : base(dataProvider, mapperManager)
        {
            UserRepository = userRepository;
        }

        public async Task<PasswordEntity> GetPasswordById(int id)
        {
            try
            {
                IEnumerable<PasswordPoco> passwordPocoEnum = await DataProvider.GetBy<PasswordPoco, int>("test_passwords", "id_password", id);
                List<PasswordPoco> passwordPocos = passwordPocoEnum.ToList();
                if (passwordPocos.Any())
                {
                    PasswordPoco passwordPoco = passwordPocos.First();
                    PasswordEntity passwordEntity = MapperManager.Mapper.Map<PasswordPoco, PasswordEntity>(passwordPoco);

                    //Est-ce que c'est vraiment comme ca que je suis suppose faire les nested objects? 
                    passwordEntity.User = await UserRepository.GetUserById(passwordEntity.UserId);
                    return passwordEntity;
                }
                else
                {
                    return null;
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
                IEnumerable<PasswordPoco> passwordPocoEnum = await DataProvider.GetBy<PasswordPoco, int>("test_passwords", "id_user", userId);
                List<PasswordPoco> passwordPocos = passwordPocoEnum.ToList();
                if (passwordPocos.Any())
                {
                    PasswordPoco passwordPoco = passwordPocos.First();
                    PasswordEntity passwordEntity = MapperManager.Mapper.Map<PasswordPoco, PasswordEntity>(passwordPoco);
                    return passwordEntity;
                }
                else
                {
                    throw new PasswordException("Unable to get [password] by [id_user] = " + userId);
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

        public void PostPassword(PasswordEntity passwordEntity)
        {
            try
            {
                PasswordPoco pP = MapperManager.Mapper.Map<PasswordEntity, PasswordPoco>(passwordEntity);
                DataProvider.Post(pP);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[PasswordRepository.PostPassword] " + e.ToString());
                throw new LoginException("Unable to create password");
            }
        }
    }
}