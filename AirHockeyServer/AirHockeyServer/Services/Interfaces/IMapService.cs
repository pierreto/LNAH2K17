using AirHockeyServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace AirHockeyServer.Services.Interfaces
{
    public interface IMapService
    {
        Task<IEnumerable<MapEntity>> GetMaps();
        Task<MapEntity> GetMap(int id);
        Task SaveMap(MapEntity map);
        Task<int?> GetMapID(MapEntity map);
    }
}