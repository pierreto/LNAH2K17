using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Linq;

namespace AirHockeyServer.DatabaseCore
{
    public class DataProvider : IDataProvider
    {
        public DataContext DC { get; set; }

        public DataProvider()
        {
            DC = new DataContext(new MySqlConnection(ConfigurationManager.ConnectionStrings["lnah"].ConnectionString));
        }

        public List<T> GetAll<T>(string table)
        {
            
            try
            {
                string query = string.Format("SELECT * FROM {0}", table);
                IEnumerable<T> po = DC.ExecuteQuery<T>(query);
                return po.ToList();

            }
            catch (Exception e)
            {
                return new List<T>();
            }
        }

        public T GetById<T>(string table, int id)
        {
            try
            {
                string query = string.Format("SELECT * FROM {0} WHERE id_user={1}", table, id);
                IEnumerable<T> po = DC.ExecuteQuery<T>(query);
                return po.ToList().First();

            }
            catch (Exception e)
            {
                return default(T);
            }
        }
    }
}