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

        Task UpdateMap(MapEntity updatedMap);

        Task<IEnumerable<MapEntity>> GetMaps();

        Task CreateNewMap(MapEntity map);

        Task<int?> GetMapID(MapEntity savedMap);

        Task<MapEntity> GetMap(int idMap);


    }
}
