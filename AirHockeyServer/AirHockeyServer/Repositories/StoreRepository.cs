using AirHockeyServer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AirHockeyServer.Entities;
using System.Threading.Tasks;
using AirHockeyServer.Mapping;
using AirHockeyServer.Pocos;
using System.Data.Linq;
using AirHockeyServer.Core;

namespace AirHockeyServer.Repositories
{
    public class StoreRepository : Repository, IStoreRepository
    {

        public StoreRepository(MapperManager mapperManager) : base(mapperManager)
        {
            MapperManager = mapperManager;

        }

        public async Task AddUserItem(int userId, StoreItemEntity item)
        {
            try
            {
                using (MyDataContext DC = new MyDataContext())
                {
                    // todo : get item
                    StoreItemEntity officialItem = Cache.StoreItems[item.Id];
                    StoreItemPoco poco = new StoreItemPoco
                    {
                        Id = item.Id,
                        UserId = userId,
                        IsGameEnabled = item.IsGameEnabled
                    };
                    
                    DC.GetTable<StoreItemPoco>().InsertOnSubmit(poco);

                    await Task.Run(() => DC.SubmitChanges());
                    
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[StoreRepository.AddUserItem] " + e.ToString());
            }
        }

        public async Task<List<StoreItemEntity>> GetUserStoreItems(int userId)
        {
            try
            {
                using (MyDataContext DC = new MyDataContext())
                {
                    IQueryable<StoreItemPoco> queryable =
                    from items in DC.GetTable<StoreItemPoco>() where items.UserId == userId select items;

                    var results = await Task.Run(
                        () => queryable.ToArray());

                    List<StoreItemPoco> result = results.ToList();

                    return GetStoreEntitiesFromPocos(result);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[StoreRepository.GetUserStoreItems] " + e.ToString());
                return null;
            }
        }

        public async Task UpdateUserItem(int userId, StoreItemEntity item)
        {
            try
            {
                using (MyDataContext DC = new MyDataContext())
                {
                    var query =
                        from items in DC.GetTable<StoreItemPoco>() where items.UserId == userId && items.Id == item.Id select items;

                    var results = query.ToArray();
                    var existingItem = results.First();

                    existingItem.IsGameEnabled = item.IsGameEnabled;

                    await Task.Run(() => DC.SubmitChanges());
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[StoreRepository.UpdateUserItem] " + e.ToString());
            }
        }

        public async Task UpdateUserItems(int userId, List<StoreItemEntity> storeItems)
        {
            try
            {
                using (MyDataContext DC = new MyDataContext())
                {
                    var query =
                        from items in DC.GetTable<StoreItemPoco>() where items.UserId == userId 
                        //&& storeItems.Any(x => x.Id == items.Id)
                        select items;

                    var results = query.ToArray();

                    foreach(var item in results)
                    {
                        var storeItem = storeItems.Find(w => item.Id == w.Id);
                        if(storeItem != null)
                        {

                        item.IsGameEnabled = storeItem.IsGameEnabled;
                        }
                    }

                    await Task.Run(() => DC.SubmitChanges());
                }
            }
            catch(Exception e)
            {

            }
        }

        private List<StoreItemEntity> GetStoreEntitiesFromPocos(List<StoreItemPoco> pocos)
        {
            List<StoreItemEntity> results = new List<StoreItemEntity>();
            foreach(var poco in pocos)
            {
                results.Add(new StoreItemEntity(poco));
            }

            return results;
        }
    }
}