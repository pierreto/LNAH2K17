using System;
using AirHockeyServer.Entities;
using System.Threading.Tasks;
using AirHockeyServer.Repositories;

namespace AirHockeyServer.Services
{
    public class PasswordService
    {
        private PasswordRepository PasswordRepository;

        public PasswordService()
        {
            try
            {
                PasswordRepository = new PasswordRepository();
            }
            catch (PasswordException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<PasswordEntity> GetPasswordById(int id)
        {
            try
            {
                return await PasswordRepository.GetPasswordById(id);
            }
            catch (PasswordException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<PasswordEntity> GetPasswordByUserId(int userId)
        {
            try
            {
                return await PasswordRepository.GetPasswordByUserId(userId);
            }
            catch (PasswordException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void PostPassword(PasswordEntity passwordEntity)
        {
            try
            {
                PasswordRepository.PostPassword(passwordEntity);
            }
            catch (PasswordException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }

    public class PasswordException : Exception
    {
        public string ErrorMessage { get; set; }
        public PasswordException(string message)
        {
            this.ErrorMessage = message;
        }
    }
}