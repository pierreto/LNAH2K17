using AirHockeyServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirHockeyServer.Repositories.Interfaces
{
    public interface IMapRepository
    {

        Task<bool> UpdateMap(MapEntity updatedMap);

        Task<IEnumerable<MapEntity>> GetMaps();

        Task<IEnumerable<MapEntity>> GetFullMaps();

        Task<int?> CreateNewMap(MapEntity map);

        Task<MapEntity> GetMap(int idMap);

        Task<bool> RemoveMap(int id);

        Task<bool> SyncMap(MapEntity map);
    }
}