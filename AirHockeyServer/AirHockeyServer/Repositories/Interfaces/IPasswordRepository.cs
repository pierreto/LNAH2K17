using AirHockeyServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AirHockeyServer.Repositories.Interfaces
{
    public interface IPasswordRepository
    {
        Task<PasswordEntity> GetPasswordById(int id);

        Task<PasswordEntity> GetPasswordByUserId(int userId);

        Task PostPassword(PasswordEntity passwordEntity);
    }
}