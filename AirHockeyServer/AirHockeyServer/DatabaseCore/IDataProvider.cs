using AirHockeyServer.Pocos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirHockeyServer.DatabaseCore
{
    public interface IDataProvider
    {
        Task<IEnumerable<T>> GetAll<T>(string table);

        Task<IEnumerable<T>> GetBy<T, K>(string table, string field, K value);

        void Post<T>(T poco) where T : Poco;

    }
}
