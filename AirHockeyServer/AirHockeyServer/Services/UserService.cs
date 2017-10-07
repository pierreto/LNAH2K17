using AirHockeyServer.Entities;
using AirHockeyServer.Repositories;
using System;
using System.Collections.Generic;

namespace AirHockeyServer.Services
{
    public class UserService
    {
        private UserRepository UserRepository;

        public UserService()
        {
            UserRepository = new UserRepository();
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

        public void PostUser()
        {
            //return UserRepository.PostUser();
        }
    }
}