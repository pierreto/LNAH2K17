using AirHockeyServer.Entities;
using AirHockeyServer.Repositories;
using AirHockeyServer.Repositories.Interfaces;
using AirHockeyServer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AirHockeyServer.Services
{
    public class UserService : IUserService
    {
        private IUserRepository UserRepository;

        public UserService(IUserRepository userRepository)
        {
            UserRepository = userRepository;
        }

        public async Task<UserEntity> GetUserById(int id)
        {
            try
            {
                return await UserRepository.GetUserById(id);
            }
            catch (UserException e)
            {
                System.Diagnostics.Debug.WriteLine("[UserService.GetUserById] " + e.ToString());
                throw e;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[UserService.GetUserById] " + e.ToString());
                throw e;
            }
        }

        public async Task<UserEntity> GetUserByUsername(string username)
        {
            try
            {
                return await UserRepository.GetUserByUsername(username);
            }
            catch (UserException e)
            {
                System.Diagnostics.Debug.WriteLine("[UserService.GetUserByUsername] " + e.ToString());
                throw e;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[UserService.GetUserByUsername] " + e.ToString());
                throw e;
            }
        }

        public async Task<List<UserEntity>> GetAllUsers()
        {
            try
            {
                return await UserRepository.GetAllUsers();
            }
            catch (UserException e)
            {
                System.Diagnostics.Debug.WriteLine("[UserService.GetAllUsers] " + e.ToString());
                throw e;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[UserService.GetAllUsers] " + e.ToString());
                throw e;
            }
        }

        public async Task PostUser(UserEntity userEntity)
        {
            try
            {
                await UserRepository.PostUser(userEntity);
            }
            catch (UserException e)
            {
                System.Diagnostics.Debug.WriteLine("[UserService.PostUser] " + e.ToString());
                throw e;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[UserService.PostUser] " + e.ToString());
                throw e;
            }
        }
    }

    public class UserException : Exception
    {
        public UserException(string message) : base(message)
        {
        }
    }
}