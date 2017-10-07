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

        public async Task<List<T>> GetAll<T>(string table)
        {
            try
            {
                string queryString = string.Format("SELECT * FROM {0}", table);
                Task<IEnumerable<T>> query = new Task<IEnumerable<T>>(() => DC.ExecuteQuery<T>(queryString));
                await query;
                return query.Result.ToList();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[DataProvider.GetAll] " + e.ToString());
                return new List<T>();
            }
        }

        public async Task<T> GetById<T>(string table, int id)
        {
            try
            {
                string queryString = string.Format("SELECT * FROM {0} WHERE id_user={1}", table, id);
                Task<IEnumerable<T>> query = new Task<IEnumerable<T>>(() => DC.ExecuteQuery<T>(queryString));
                await query;
                return query.Result.ToList().First();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[DataProvider.GetById] " + e.ToString());
                return default(T);
            }
        }
    }
}