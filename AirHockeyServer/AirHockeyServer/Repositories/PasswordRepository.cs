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

        public async Task<PasswordEntity> GetPasswordById(int id)
        {
            try
            {
                IEnumerable<PasswordPoco> passwordPocoEnum = await DataProvider.GetById<PasswordPoco>("test_passwords", "id_password", id);
                PasswordPoco passwordPoco = passwordPocoEnum.ToList().First();
                PasswordEntity passwordEntity = MapperManager.Map<PasswordPoco, PasswordEntity>(passwordPoco);

                //Est-ce que c'est vraiment comme ca que je suis suppose faire les nested objects? 
                IEnumerable<UserPoco> userPocoEnum = await DataProvider.GetById<UserPoco>("test_users", "id_user", passwordEntity.UserId);
                UserPoco userPoco = userPocoEnum.ToList().First();
                UserEntity userEntity = MapperManager.Map<UserPoco, UserEntity>(userPoco);

                passwordEntity.User = userEntity;
                return passwordEntity;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[PasswordRepository.GetPasswordById] " + e.ToString());
                return null;
            }
        }
    }
}