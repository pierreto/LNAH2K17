﻿using AirHockeyServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AirHockeyServer.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserEntity> GetUserById(int id);

        Task<UserEntity> GetUserByUsername(string username);

        Task<List<UserEntity>> GetAllUsers();

        void PostUser(UserEntity userEntity);
    }
}