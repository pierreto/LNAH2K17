using AirHockeyServer.DatabaseCore;
using AirHockeyServer.Entities;
using AirHockeyServer.Repositories;
using AirHockeyServer.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace AirHockeyServer.Services
{
    public class SignupService: ISignupService, IService
    {
        private IUserService UserService { get; set; }
        private IPasswordService PasswordService { get; set; }
        public IPlayerStatsService PlayerStatsService { get; set; }

        public SignupService(IUserService userService, IPasswordService passwordService, IPlayerStatsService playerStatsService)
        {
            UserService = userService;
            PasswordService = passwordService;
            PlayerStatsService = playerStatsService;
        }

        public async Task<int> Signup(SignupEntity signupEntity)
        {
            try
            {
                UserEntity uE = await UserService.GetUserByUsername(signupEntity.Username);
                if (uE == null) //Username not already taken
                {
                    //TODO: englober tout ceci dans une transaction au cas ou le postUser fonctionne, mais pas le postPassword
                    uE = new UserEntity { Username = signupEntity.Username };
                    await UserService.PostUser(uE);
                    //TODO: essayer d'avoir le retour du id au moment du POST a la place
                    UserEntity uE2 = await UserService.GetUserByUsername(uE.Username);
                    PasswordEntity pE = new PasswordEntity { UserId = uE2.Id, Password = signupEntity.Password };
                    await PasswordService.PostPassword(pE);

                    await PlayerStatsService.SetPlayerAchievements(uE2.Id);

                    return uE2.Id;
                }
                else
                {
                    throw new SignupException("Ce nom d'utilisateur existe déjà");
                }
            }
            catch (SignupException e)
            {
                System.Diagnostics.Debug.WriteLine("[SignupService.Signup] " + e.ToString());
                throw e;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[SignupService.Signup] " + e.ToString());
                throw e;
            }
        }
    }

    public class SignupException : Exception
    {
        public SignupException(string message) : base(message)
        {
        }
    }
}