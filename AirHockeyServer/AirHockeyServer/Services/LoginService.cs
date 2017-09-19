using System;
using System.Collections.Generic;
using AirHockeyServer.Entities;
using Microsoft.Ajax.Utilities;

namespace AirHockeyServer.Services
{
    public class LoginService : ILoginService, IService
    {
        //This is for TESTING purpose only, we should not do this for remembering users that are connected
        private List<string> membersList;

        public LoginService()
        {
            membersList = new List<string>();
        }

        public void login(LoginFormMessage loginForm) 
        {
            if (membersList.Contains(loginForm.username))
            {
                throw new LoginException("Login name already taken."); 
            }
            this.membersList.Add(loginForm.username);

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