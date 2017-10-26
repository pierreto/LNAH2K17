using AirHockeyServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace AirHockeyServer.Services
{
    public interface IMapService
    {
        Task<IEnumerable<MapEntity>> GetMaps();
        Task<MapEntity> GetMapByName(string creator, string name);
        Task SaveMap(MapEntity map);
    }
}