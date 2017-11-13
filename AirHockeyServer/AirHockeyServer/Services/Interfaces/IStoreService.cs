using AirHockeyServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AirHockeyServer.Services.Interfaces
{
    public interface IStoreService
    {
        List<StoreItemEntity> GetStoreItems();

        Task<List<StoreItemEntity>> GetUserStoreItems(int userId);

        Task UpdateUserItem(int userId, StoreItemEntity item);

        Task AddUserItem(int userId, StoreItemEntity item);


    }
}