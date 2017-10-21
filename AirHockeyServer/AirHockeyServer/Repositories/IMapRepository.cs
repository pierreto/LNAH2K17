using AirHockeyServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirHockeyServer.Repositories
{
    public interface IMapRepository
    {
        Task<MapEntity> GetMapByName(string creator, string name);

        Task CreateNewMap(MapEntity map);

        Task UpdateMap(MapEntity updatedMap);
    }
}
