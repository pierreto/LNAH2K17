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

        public async Task<MapEntity> GetMap(int idMap)
        {
            try
            {
                IQueryable<MapPoco> queryable =
                    from map in this.Maps where map.Id == idMap select map;

                var results = await Task<IEnumerable<MapPoco>>.Run(
                    () => queryable.ToArray());

                MapPoco result = results.Length > 0 ? results.First() : null;
                return MapperManager.Map<MapPoco, MapEntity>(result);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[MapRepository.GetMap] " + e.ToString());
                return null;
            }
        }

        public async Task<IEnumerable<MapEntity>> GetMaps()
        {
            try
            {
                IQueryable<MapPoco> queryable = from map in this.Maps select map;
                List<MapPoco> maps = await Task<List<MapPoco>>.Run(
                    () => queryable.ToList());

                List<MapEntity> mapEntities = MapperManager.Map<List<MapPoco>, List<MapEntity>>(maps);
                for (int i = 0; i < mapEntities.Count(); i++)
                {
                    mapEntities[i].Json = null;
                }

                return mapEntities;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[MapRepository.GetMaps] " + e.ToString());
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

                var query =
                    from _map in this.Maps
                    where _map.Creator == map.Creator && _map.Name == map.MapName && _map.CreationDate == map.LastBackup
                    select _map;
                var results = await Task<MapPoco>.Run(
                    () => query.ToArray());

                MapPoco result = results.Length > 0 ? results.First() : null;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[MapRepository.CreateNewMap] " + e.ToString());
            }
        }

        public async Task<int?> GetMapID(MapEntity savedMap)
        {
            try
            {
                var query =
                    from map in this.Maps
                    where map.Creator == savedMap.Creator && map.Name == savedMap.MapName
                    select map;
                var results = await Task<MapPoco>.Run(
                    () => query.ToArray());

                MapPoco result = results.Length > 0 ? results.First() : null;
                return result?.Id;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[MapRepository.GetMapID] " + e.ToString());
                return null;
            }
        }

        public async Task UpdateMap(MapEntity updatedMap)
        {
            try
            {
                MapPoco _updatedMap = MapperManager.Map<MapEntity, MapPoco>(updatedMap);
                var query =
                    from map in this.Maps where map.Id == updatedMap.Id select map;
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