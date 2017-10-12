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

        public void PostUser(UserEntity userEntity)
        {
            try
            {
                UserRepository.PostUser(userEntity);
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