using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Web;
using AirHockeyServer.Entities;
using AirHockeyServer.Pocos;
using System.Threading.Tasks;
using AirHockeyServer.Mapping;
using AirHockeyServer.Repositories.Interfaces;

namespace AirHockeyServer.Repositories
{
    public class MapRepository : IMapRepository
    {

        MapperManager MapperManager { get; set; }

        public MapRepository(MapperManager mapperManager)
        {
            MapperManager = mapperManager;
        }

        public async Task<MapEntity> GetMap(int idMap)
        {
            try
            {
                using (MyDataContext DC = new MyDataContext())
                {
                    var query =
                        from map in DC.MapsTable where map.Id == idMap select map;

                    var results = await Task<IEnumerable<MapPoco>>.Run(
                        () => query.ToArray());

                    MapPoco result = results.Length > 0 ? results.First() : null;
                    return MapperManager.Map<MapPoco, MapEntity>(result);
                }
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
                using (MyDataContext DC = new MyDataContext())
                {
                    var query = from map in DC.MapsTable select map;
                    var maps = await Task<List<MapPoco>>.Run(
                        () => query.ToList());

                    List<MapEntity> mapEntities = MapperManager.Map<List<MapPoco>, List<MapEntity>>(maps);
                    for (int i = 0; i < mapEntities.Count(); i++)
                    {
                        mapEntities[i].Json = null;
                    }

                    return mapEntities;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[MapRepository.GetMaps] " + e.ToString());
                return null;
            }
        }

        public async Task<int?> CreateNewMap(MapEntity map)
        {
            try
            {
                using (MyDataContext DC = new MyDataContext())
                {
                    MapPoco newMap = MapperManager.Map<MapEntity, MapPoco>(map);
                    DC.MapsTable.InsertOnSubmit(newMap);
                    await Task.Run(() => DC.SubmitChanges());

                    // To fetch the db auto-generated Id of the new created map, we have to compare
                    // creation dates without considering milliseconds (or it will fail):
                    map.CreationDate = new DateTime(
                        map.CreationDate.Year,
                        map.CreationDate.Month,
                        map.CreationDate.Day,
                        map.CreationDate.Hour,
                        map.CreationDate.Minute,
                        map.CreationDate.Second,
                        map.CreationDate.Kind);

                    // We have to fetch the db auto-generated Id of the new created map:
                    var query =
                        from _map in DC.MapsTable
                        where _map.Creator == map.Creator && _map.CreationDate == map.CreationDate
                        select _map.Id;

                    var results = await Task<List<int?>>.Run(
                        () => query.ToList<int?>());

                    return results.First();
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[MapRepository.CreateNewMap] " + e.ToString());
                return null;
            }
        }

        public async Task<bool> UpdateMap(MapEntity updatedMap)
        {
            try
            {
                using (MyDataContext DC = new MyDataContext())
                {
                    MapPoco _updatedMap = MapperManager.Map<MapEntity, MapPoco>(updatedMap);
                    var query = from map in DC.MapsTable where map.Id == updatedMap.Id select map;
                    var results = query.ToArray();
                    var existingMap = results.First();
                    if (updatedMap.Json != null)
                    {
                        existingMap.Json = updatedMap.Json;
                        existingMap.LastBackup = updatedMap.LastBackup;
                    }
                    if (updatedMap.Icon != null)
                    {
                        existingMap.Icon = updatedMap.Icon;
                    }
                    await Task.Run(() => DC.SubmitChanges());
                    return true;
                }
            }
            catch (Exception e)
            {

                System.Diagnostics.Debug.WriteLine("[MapRepository.UpdateMap] " + e.ToString());
                return false;
            }
        }

        public async Task<bool> RemoveMap(int id)
        {
            try
            {
                using (MyDataContext DC = new MyDataContext())
                {
                    var query = from map in DC.MapsTable where map.Id == id select map;
                    var results = query.ToArray();
                    var _map = results.First();
                    DC.MapsTable.DeleteOnSubmit(_map);
                    await Task.Run(() => DC.SubmitChanges());
                    return true;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[MapRepository.RemoveMap] " + e.ToString());
                return false;
            }
        }
    }
}