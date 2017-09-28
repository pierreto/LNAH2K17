using System;
using System.Collections.Generic;
using AirHockeyServer.Entities;
using Microsoft.Ajax.Utilities;

namespace AirHockeyServer.Services
{
    public class LoginService : ILoginService, IService
    {
        private static HashSet<string> _usernames = new HashSet<string>();
 
        public LoginService()
        {
        }

        public void login(LoginMessage message) 
        {
            System.Diagnostics.Debug.WriteLine(message.username);
            System.Diagnostics.Debug.WriteLine(_usernames.Count);
            if (_usernames.Contains(message.username))
            {
                throw new LoginException("Username already taken."); 
            }
            _usernames.Add(message.username);
        }

        public void disconnect()
        {
            throw new System.NotImplementedException();
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