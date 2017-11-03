using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using AirHockeyServer.Entities;
using System.Data.Linq;
using MySql.Data.MySqlClient;
using AirHockeyServer.Pocos;
using AirHockeyServer.Repositories.Interfaces;

namespace AirHockeyServer.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        public LoginRepository()
        {
        }

        public async Task<LoginEntity> GetLogins()
        {
            var connectionString = @"Server=37.187.19.181;database=log3900;uid=log3900;password=labasedesdecales;";
            DataContext dc = new DataContext(connectionString);

            dc.ExecuteQuery<LoginPoco>("SELECT * from");

            return new LoginEntity();
        }
    }
}