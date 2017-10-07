using AirHockeyServer.DatabaseCore;
using AirHockeyServer.Entities;
using AirHockeyServer.Mapping;
using AirHockeyServer.Pocos;
using AutoMapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AirHockeyServer.Repositories
{
    public class UserRepository //Inherits Repository
    {
        protected DataProvider DataProvider { get; set; } //Move to Repository

        protected MapperManager MapperManager { get; set; } 
        private UserRepository()
        {
            DataProvider = new DataProvider();
            MapperManager = new MapperManager();
        }

        private static UserRepository _instance = null;
        public static UserRepository Instance()
        {
            if (_instance == null)
                _instance = new UserRepository();
            return _instance;
        }

    //public async Task<UserEntity> GetUserById(int id)
    public UserEntity GetUserById(int id)
        {
            try
            {
                UserPoco userPoco = DataProvider.GetById<UserPoco>("test_users", id);
                UserEntity userEntity = MapperManager.Map<UserPoco, UserEntity>(userPoco);
                return userEntity;
            }
            catch (Exception w)
            {
                return null;
            }
            
        }

        public async Task<List<UserEntity>> GetAllUsers()
        {
            try
            {
                var userPocos = DataProvider.GetAll<UserPoco>("test_users");
                List<UserEntity> userEntities = MapperManager.Map<UserPoco,UserEntity>(userPocos);
                return userEntities;
            }
            catch
            {
                return null;
            }
        }
    }
}