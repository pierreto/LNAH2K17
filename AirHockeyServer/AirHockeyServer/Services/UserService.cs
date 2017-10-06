using AirHockeyServer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirHockeyServer.Services
{
    public class UserService
    {
        private static UserRepository UserRepository = new UserRepository();
        public UserService()
        {
        }

        public void GetUser()
        {
            UserRepository.GetUsers();
        }
    }
}