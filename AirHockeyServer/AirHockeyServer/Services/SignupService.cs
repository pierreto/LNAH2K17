using AirHockeyServer.DatabaseCore;
using AirHockeyServer.Entities;
using AirHockeyServer.Repositories;
using System;
using System.Threading.Tasks;

namespace AirHockeyServer.Services
{
    public class SignupService
    {
        private UserRepository UserRepository = new UserRepository();
        private PasswordRepository PasswordRepository = new PasswordRepository();
        private DataProvider DataProvider = new DataProvider();

        public async Task<bool> Signup(SignupEntity signupEntity)
        {
            //TODO: add to db
            UserEntity uE = await UserRepository.GetUserByUsername(signupEntity.Username);
            if(uE == null) //Username not already taken
            {
                uE = new UserEntity { Username = signupEntity.Username };
                UserRepository.PostUser(uE);
                //TODO: essayer d'avoir le retour du id au moment du POST a la place
                uE = await UserRepository.GetUserByUsername(uE.Username);
                PasswordEntity pE = new PasswordEntity { UserId = uE.Id, Password = signupEntity.Password };
                PasswordRepository.PostPassword(pE);
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