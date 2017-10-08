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
            catch(UserException e)
            {
                throw e;
            }
            catch (Exception e)
            {
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
                throw e;
            }
            catch (Exception e)
            {
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
                throw e;
            }
            catch (Exception e)
            {
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
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }

    public class UserException : Exception
    {
        public string ErrorMessage { get; set; }
        public UserException(string message)
        {
            this.ErrorMessage = message;
        }
    }
}