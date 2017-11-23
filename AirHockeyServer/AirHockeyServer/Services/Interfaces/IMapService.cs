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
        Task<int?> SaveNewMap(MapEntity map);
        Task<bool> SaveMap(MapEntity map);
        Task<bool> RemoveMap(int id);
        Task<bool> SyncMap(MapEntity map);
    }
}