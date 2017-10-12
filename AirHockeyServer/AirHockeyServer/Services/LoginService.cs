using System;
using System.Collections.Generic;
using AirHockeyServer.Entities;
using Microsoft.Ajax.Utilities;
using AirHockeyServer.Repositories;

namespace AirHockeyServer.Services
{
    public class LoginService : ILoginService, IService
    {
        private static HashSet<string> _usernames = new HashSet<string>();
        private static LoginRepository LoginRepository = new LoginRepository();
        public LoginService()
        {
        }

        public void Login(LoginEntity loginEntity) 
        {
            LoginRepository.GetLogins();
            System.Diagnostics.Debug.WriteLine(loginEntity.User);
            System.Diagnostics.Debug.WriteLine(loginEntity.Password);
            System.Diagnostics.Debug.WriteLine(_usernames.Count);
            if (_usernames.Contains(loginEntity.User.Username))
            {
                throw new LoginException("Username already taken."); 
            }
            _usernames.Add(loginEntity.User.Username);
        }

        public void Logout(LoginEntity loginEntity)
        {
            if (_usernames.Contains(loginEntity.User.Username))
            {
                _usernames.Remove(loginEntity.User.Username);
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