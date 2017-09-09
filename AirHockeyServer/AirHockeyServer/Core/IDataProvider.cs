using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AirHockeyServer.Core
{
    public interface IDataProvider
    {
        Task<T> GetEntity<T>(string url);

        Task<List<T>> GetEntities<T>(string url);
    }
}