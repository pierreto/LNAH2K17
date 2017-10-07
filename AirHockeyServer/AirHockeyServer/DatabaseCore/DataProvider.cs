using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Linq;
using System.Threading.Tasks;

namespace AirHockeyServer.DatabaseCore
{
    public class DataProvider : IDataProvider
    {
        public DataContext DC { get; set; }

        public DataProvider()
        {
            DC = new DataContext(new MySqlConnection(ConfigurationManager.ConnectionStrings["lnah"].ConnectionString));
        }

        public async Task<IEnumerable<T>> GetAll<T>(string table)
        {
            try
            {
                string queryString = string.Format("SELECT * FROM {0}", table);
                return await DoQuery<T>(queryString);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[DataProvider.GetAll] " + e.ToString());
                return new List<T>();
            }
        }
        private Task<IEnumerable<T>> DoQuery<T>(string queryString)
        {
            return Task<IEnumerable<T>>.Run(() =>
                DC.ExecuteQuery<T>(queryString)
            );
        }

        public async Task<IEnumerable<T>> GetById<T>(string table, int id)
        {
            try
            {
                string queryString = string.Format("SELECT * FROM {0} WHERE id_user={1}", table, id);
                return await DoQuery<T>(queryString);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[DataProvider.GetById] " + e.ToString());
                return default(IEnumerable<T>);
            }
        }
    }
}