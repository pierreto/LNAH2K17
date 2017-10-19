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

        Task<IEnumerable<T>> GetById<T>(string table, int id);
        
    }
}
