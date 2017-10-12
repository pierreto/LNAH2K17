using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Web;
using AirHockeyServer.DatabaseCore;
using AirHockeyServer.Entities;
using AirHockeyServer.Pocos;
using System.Threading.Tasks;
using AirHockeyServer.Mapping;

namespace AirHockeyServer.Repositories
{
    public class MapRepository : Repository<MapRepository>
    {
        private Table<MapPoco> Maps;

        public MapRepository()
        {
            Maps = DataProvider.DC.GetTable<MapPoco>();
        }

        public async Task<MapEntity> GetMapByName(string creator, string name)
        {
            try
            {
                IQueryable<MapPoco> queryable =
                    from map in this.Maps where map.Creator == creator && map.Name == name select map;

                var results = await Task<IEnumerable<MapPoco>>.Run(
                    () => queryable.ToArray());

                MapPoco result = results.Length > 0 ? results.First() : null;
                return MapperManager.Map<MapPoco, MapEntity>(result);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[MapRepository.GetMapByName] " + e.ToString());
                return null;
            }
        }

        public async Task CreateNewMap(MapEntity map)
        {
            try
            {
                MapPoco newMap = MapperManager.Map<MapEntity, MapPoco>(map);
                this.Maps.InsertOnSubmit(newMap);
                await Task.Run(() => this.DataProvider.DC.SubmitChanges());
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[MapRepository.CreateNewMap] " + e.ToString());
            }
        }

        public async Task UpdateMap(MapEntity updatedMap)
        {
            try
            {
                MapPoco _updatedMap = MapperManager.Map<MapEntity, MapPoco>(updatedMap);
                var query =
                    from map in this.Maps where map.Creator == _updatedMap.Creator && map.Name == _updatedMap.Name select map;
                var results = query.ToArray();
                var existingMap = results.First();
                existingMap.Json = updatedMap.Json;
                await Task.Run(() => this.DataProvider.DC.SubmitChanges());
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[MapRepository.UpdateMap] " + e.ToString());
            }
        }
    }
}