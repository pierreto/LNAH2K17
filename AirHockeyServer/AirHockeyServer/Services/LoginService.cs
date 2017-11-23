using System;
using System.Collections.Generic;
using AirHockeyServer.Entities;
using AirHockeyServer.Repositories;
using System.Threading.Tasks;
using AirHockeyServer.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace AirHockeyServer.Services
{
    public class LoginService : ILoginService, IService
    {
        private static HashSet<string> _usernames = new HashSet<string>();
        private IUserService UserService { get; set; }
        private IPasswordService PasswordService { get; set; }

        public LoginService(IUserService userService, IPasswordService passwordService)
        {
            UserService = userService;
            PasswordService = passwordService;
        }

        public async Task<int> ValidateCredentials(LoginEntity loginEntity)
        {
            try
            {
                UserEntity uE = await UserService.GetUserByUsername(loginEntity.Username);
                if (uE != null)
                {
                    PasswordEntity pE = await PasswordService.GetPasswordByUserId(uE.Id);
                    if (pE != null)
                    {
                        var sha1 = new SHA1CryptoServiceProvider();
                        string providedPassword =
                            Convert.ToBase64String(
                                sha1.ComputeHash(
                                    Encoding.UTF8.GetBytes(loginEntity.Password)));

                        if (uE.Username != loginEntity.Username || pE.Password != providedPassword)
                        {
                            throw new LoginException("Nom d'usager ou mot de passe invalide");
                        }
                        else
                        {
                            if (!loginEntity.LoginFromWebApp)
                            {
                                if (_usernames.Contains(loginEntity.Username))
                                {
                                    throw new LoginException("Déjà connecté");
                                }
                                _usernames.Add(loginEntity.Username);
                            }
                            return uE.Id;
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
            if (!loginEntity.LoginFromWebApp)
            {
                if (_usernames.Contains(loginEntity.Username))
                {
                    _usernames.Remove(loginEntity.Username);
                }
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