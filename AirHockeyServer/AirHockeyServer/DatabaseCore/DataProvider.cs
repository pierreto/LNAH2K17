using AirHockeyServer.Pocos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.IO;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace AirHockeyServer.DatabaseCore
{
    public class DataProvider : IDataProvider
    {
        public MyDataContext DC { get; set; }

        public DataProvider()
        {
            DC = new MyDataContext();
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

        // Not async?
        public void Post<T>(T poco) where T: Poco
        {
            try
            {
                DC.GetTable<T>().InsertOnSubmit(poco);
                DC.SubmitChanges();
                System.Diagnostics.Debug.WriteLine("Poco id : " + poco.Id);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[DataProvider.Post] " + e.ToString());
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