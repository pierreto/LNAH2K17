﻿using System;
using System.Collections.Generic;
using AirHockeyServer.Entities;
using AirHockeyServer.Repositories;
using System.Threading.Tasks;

namespace AirHockeyServer.Services
{
    public class LoginService : ILoginService, IService
    {
        private static HashSet<string> _usernames = new HashSet<string>();
        private UserService UserService = new UserService();
        private PasswordService PasswordService = new PasswordService();

        public async Task ValidateCredentials(LoginEntity loginEntity)
        {
            try
            {
                UserEntity uE = await UserService.GetUserByUsername(loginEntity.Username);
                if (uE != null)
                {
                    PasswordEntity pE = await PasswordService.GetPasswordByUserId(uE.Id);
                    if (pE != null)
                    {
                        if (uE.Username != loginEntity.Username || pE.Password != loginEntity.Password)
                        {
                            throw new LoginException("Nom d'usager ou mot de passe invalide");
                        }
                    }
                    else
                    {
                        throw new LoginException("Impossible de trouver votre mot de passe");
                    }
                }
                else
                {
                    throw new LoginException("Nom d'usager ou mot de passe invalide");
                }
                //if (_usernames.Contains(loginEntity.Username))
                //{
                //    throw new LoginException("Username already taken.");
                //}
                //_usernames.Add(loginEntity.Username);
                //return false;
            }
            catch (LoginException e)
            {
                System.Diagnostics.Debug.WriteLine("[LoginService.ValidateCredentials] " + e.ToString());
                throw e;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[LoginService.ValidateCredentials] " + e.ToString());
                throw e;
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
        public LoginException(string message) : base(message)
        {
        }
    }
}