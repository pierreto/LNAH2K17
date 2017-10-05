using AirHockeyServer.DatabaseCore;
using AirHockeyServer.Entities;
using AirHockeyServer.Pocos;
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
    public class UserRepository
    {


        public UserRepository()
        {
        }

        public async Task<List<UserEntity>> GetUsers()
        {
            DataContext dc = new DataContext(new MySqlConnection(ConfigurationManager.ConnectionStrings["lnah"].ConnectionString));
            // Do the SQL call
            try
            {
                IEnumerable<UserPoco> po = dc.ExecuteQuery<UserPoco>("SELECT id_user, username FROM test_users");
                List<UserPoco> poList = po.ToList();
            }
            catch (Exception e)
            {

            }
            

            // UserPoco will be mapped to UserEntity?
            return new List<UserEntity>();
        }
    }
}