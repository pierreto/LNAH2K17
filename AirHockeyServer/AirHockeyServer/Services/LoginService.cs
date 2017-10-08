using System;
using System.Collections.Generic;
using AirHockeyServer.Entities;
using AirHockeyServer.Repositories;
using System.Threading.Tasks;

namespace AirHockeyServer.Services
{
    public class LoginService : ILoginService, IService
    {
        private static HashSet<string> _usernames = new HashSet<string>();
        private UserRepository UserRepository = new UserRepository();
        private PasswordRepository PasswordRepository = new PasswordRepository();
        public LoginService()
        {
            PasswordRepository = new PasswordRepository();
        }

        public async Task<bool> ValidateCredentials(LoginEntity loginEntity)
        {
            try
            {
                UserEntity uE = await UserRepository.GetUserByUsername(loginEntity.Username);
                PasswordEntity pE = await PasswordRepository.GetPasswordByUserId(uE.Id);
                if (uE.Username == loginEntity.Username && pE.Password == loginEntity.Password)
                {
                    System.Diagnostics.Debug.WriteLine("Successful Login");
                    return true;
                }
                else
                {
                    return false;
                }
                //if (_usernames.Contains(loginEntity.Username))
                //{
                //    throw new LoginException("Username already taken.");
                //}
                //_usernames.Add(loginEntity.Username);
                //return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public void Logout(LoginEntity loginEntity)
        {
            if (_usernames.Contains(loginEntity.Username))
            {
                _usernames.Remove(loginEntity.Username);
            }
        }
    }

    public class LoginException : Exception
    {
        private string message;
        public LoginException(string message)
        {
            this.message = message;
        }
    }
}