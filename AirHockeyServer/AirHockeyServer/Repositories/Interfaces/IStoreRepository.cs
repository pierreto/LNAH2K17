using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AirHockeyServer.Entities;

namespace AirHockeyServer.Repositories.Interfaces
{
    public interface IStoreRepository
    {
        Task AddUserItem(int userId, StoreItemEntity item);

        Task<List<StoreItemEntity>> GetUserStoreItems(int userId);

        Task UpdateUserItem(int userId, StoreItemEntity item);

        Task UpdateUserItems(int userId, List<StoreItemEntity> storeItems);
    }
}