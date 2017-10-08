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

        public async Task<bool> Signup(SignupEntity signupEntity)
        {
            //TODO: add to db
            UserEntity uE = await UserService.GetUserByUsername(signupEntity.Username);
            if(uE == null) //Username not already taken
            {
                //TODO: englober tout ceci dans une transaction au cas ou le postUser fonctionne, mais pas le postPassword
                uE = new UserEntity { Username = signupEntity.Username };
                UserService.PostUser(uE);
                //TODO: essayer d'avoir le retour du id au moment du POST a la place
                uE = await UserService.GetUserByUsername(uE.Username);
                PasswordEntity pE = new PasswordEntity { UserId = uE.Id, Password = signupEntity.Password };
                PasswordService.PostPassword(pE);
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class SignupException : Exception
    {
        private string message;
        public SignupException(string message)
        {
            this.message = message;
        }
    }
}