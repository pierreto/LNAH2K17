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

        public async Task<IEnumerable<T>> GetBy<T,K>(string table, string field, K value)
        {
            try
            {
                //TODO: A Revoir le String format. P-e qu'il vaut mieux faire des classes pour chaque provider
                string queryString = "";
                if (typeof(K).Name.Equals("String"))
                {
                    queryString = string.Format("SELECT * FROM {0} WHERE {1}='{2}'", table, field, value);
                }
                else
                {
                    queryString = string.Format("SELECT * FROM {0} WHERE {1}={2}", table, field, value);
                }
                return await DoQuery<T>(queryString);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[DataProvider.GetById] " + e.ToString());
                return default(IEnumerable<T>);
            }
        }

        private Task<IEnumerable<T>> DoQuery<T>(string queryString)
        {
            return Task<IEnumerable<T>>.Run(() =>
                DC.ExecuteQuery<T>(queryString)
            );
        }
    }
}