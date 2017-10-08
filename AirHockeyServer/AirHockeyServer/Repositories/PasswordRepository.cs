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
    public class PasswordRepository : Repository<PasswordRepository>
    {
        UserRepository UserRepository;

        public PasswordRepository()
        {
            UserRepository = new UserRepository();
        }
           
        public async Task<PasswordEntity> GetPasswordById(int id)
        {
            try
            {
                IEnumerable<PasswordPoco> passwordPocoEnum = await DataProvider.GetBy<PasswordPoco, int>("test_passwords", "id_password", id);
                PasswordPoco passwordPoco = passwordPocoEnum.ToList().First();
                PasswordEntity passwordEntity = MapperManager.Mapper.Map<PasswordPoco, PasswordEntity>(passwordPoco);

                //Est-ce que c'est vraiment comme ca que je suis suppose faire les nested objects? 
                passwordEntity.User = await UserRepository.GetUserById(passwordEntity.UserId);
                return passwordEntity;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[PasswordRepository.GetPasswordById] " + e.ToString());
                return null;
            }
        }

        public async Task<PasswordEntity> GetPasswordByUserId(int id)
        {
            try
            {
                IEnumerable<PasswordPoco> passwordPocoEnum = await DataProvider.GetBy<PasswordPoco, int>("test_passwords", "id_user", id);
                PasswordPoco passwordPoco = passwordPocoEnum.ToList().First();
                PasswordEntity passwordEntity = MapperManager.Mapper.Map<PasswordPoco, PasswordEntity>(passwordPoco);
                return passwordEntity;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[PasswordRepository.GetPasswordByUserId] " + e.ToString());
                return null;
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
            }
        }
    }
}