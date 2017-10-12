using AirHockeyServer.Pocos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.IO;
using System.Threading.Tasks;

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
                throw new UserException("Unable to find [" + typeof(T).Name + "s] in table [" + table + "]");
            }
        }

        // Not async?
        public void Post<T>(T poco) where T : Poco
        {
            try
            {
                DC.GetTable<T>().InsertOnSubmit(poco);
                DC.SubmitChanges();
                System.Diagnostics.Debug.WriteLine("POCO ID: " + poco.Id);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[DataProvider.Post] " + e.ToString());
                throw new UserException("Unable to post [" + typeof(T).Name + "]");
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