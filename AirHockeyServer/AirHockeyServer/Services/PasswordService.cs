using System;
using AirHockeyServer.Entities;
using System.Threading.Tasks;
using AirHockeyServer.Repositories;
using AirHockeyServer.Repositories.Interfaces;
using AirHockeyServer.Services.Interfaces;

namespace AirHockeyServer.Services
{
    public class PasswordService : IPasswordService
    {
        private IPasswordRepository PasswordRepository;

        public PasswordService(IPasswordRepository passwordRepository)
        {
            PasswordRepository = passwordRepository;
        }

        public async Task<PasswordEntity> GetPasswordById(int id)
        {
            try
            {
                return await PasswordRepository.GetPasswordById(id);
            }
            catch (PasswordException e)
            {
                System.Diagnostics.Debug.WriteLine("[PasswordService.GetPasswordById] " + e.ToString());
                throw e;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[PasswordService.GetPasswordById] " + e.ToString());
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
                System.Diagnostics.Debug.WriteLine("[PasswordService.GetPasswordByUserId] " + e.ToString());
                throw e;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[PasswordService.GetPasswordByUserId] " + e.ToString());
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
                System.Diagnostics.Debug.WriteLine("[PasswordService.PostPassword] " + e.ToString());
                throw e;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[PasswordService.PostPassword] " + e.ToString());
                throw e;
            }
        }
    }

    public class PasswordException : Exception
    {
        public PasswordException(string message) : base(message)
        {
        }
    }
}