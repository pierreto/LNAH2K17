using AirHockeyServer.Entities;
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

        public UserEntity GetUserById(int id)
        {
            return UserRepository.GetUserById(id);
        }

        public List<UserEntity> GetAllUsers()
        {
            return new List<UserEntity>();
           // return UserRepository.GetAllUsers();
        }
    }
}