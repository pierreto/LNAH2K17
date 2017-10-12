using AirHockeyServer.DatabaseCore;
using AirHockeyServer.Entities;
using AirHockeyServer.Repositories;
using System;
using System.Threading.Tasks;

namespace AirHockeyServer.Services
{
    public class SignupService
    {
        private UserService UserService = new UserService();
        private PasswordService PasswordService = new PasswordService();

        public async Task Signup(SignupEntity signupEntity)
        {
            try
            {
                UserEntity uE = await UserService.GetUserByUsername(signupEntity.Username);
                if (uE == null) //Username not already taken
                {
                    //TODO: englober tout ceci dans une transaction au cas ou le postUser fonctionne, mais pas le postPassword
                    uE = new UserEntity { Username = signupEntity.Username };
                    UserService.PostUser(uE);
                    //TODO: essayer d'avoir le retour du id au moment du POST a la place
                    UserEntity uE2 = await UserService.GetUserByUsername(uE.Username);
                    PasswordEntity pE = new PasswordEntity { UserId = uE2.Id, Password = signupEntity.Password };
                    PasswordService.PostPassword(pE);
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