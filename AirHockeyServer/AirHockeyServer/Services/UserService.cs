using AirHockeyServer.Entities;
using AirHockeyServer.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AirHockeyServer.Services
{
    public class UserService
    {
        private UserRepository UserRepository;

        public UserService()
        {
            UserRepository = new UserRepository();
        }

        public async Task<UserEntity> GetUserById(int id)
        {
            return await UserRepository.GetUserById(id);
        }

        public async Task<List<UserEntity>> GetAllUsers()
        {
           return await UserRepository.GetAllUsers();
        }

        public void PostUser(UserEntity userEntity)
        {
            UserRepository.PostUser(userEntity);
        }
    }
}