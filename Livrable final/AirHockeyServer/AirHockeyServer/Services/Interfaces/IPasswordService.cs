using AirHockeyServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AirHockeyServer.Services.Interfaces
{
    public interface IPasswordService
    {
        Task<PasswordEntity> GetPasswordByUserId(int userId);

        Task PostPassword(PasswordEntity passwordEntity);

        Task<PasswordEntity> GetPasswordById(int id);
    }
}