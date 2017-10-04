using AirHockeyServer.DatabaseCore;
using AirHockeyServer.Entities;
using AirHockeyServer.Pocos;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AirHockeyServer.Repositories
{
    public class UserRepository
    {


        public UserRepository()
        {
        }

        public async Task<List<UserEntity>> GetUsers()
        {
            var connectionString = @"Server=37.187.19.181;database=log3900;uid=log3900;password=labasedesdecales;";
            DataContext dc = new DataContext(connectionString);
            // Do the SQL call
            IEnumerable<UserPoco> po = dc.ExecuteQuery<UserPoco>("SELECT username FROM test_users");

            DataContext de = new DataContext(connectionString);
            // UserPoco will be mapped to UserEntity?
            return new List<UserEntity>();
        }
    }
}