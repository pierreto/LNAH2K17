using AirHockeyServer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AirHockeyServer.Entities;
using System.Threading.Tasks;
using AirHockeyServer.Repositories.Interfaces;
using AirHockeyServer.Core;

namespace AirHockeyServer.Services
{
    public class StoreService : IStoreService
    {
        public StoreService(IStoreRepository storeRepository, IPlayerStatsService playerStatsService)
        {
            StoreRepository = storeRepository;
            PlayerStatsService = playerStatsService;
        }

        public IStoreRepository StoreRepository { get; }
        public IPlayerStatsService PlayerStatsService { get; }

        public async Task AddUserItems(int userId, List<StoreItemEntity> items)
        {
            foreach(var item in items)
            {
                await StoreRepository.AddUserItem(userId, item);
            }

            int totalPrice = items.Sum(x => x.Price);
            await PlayerStatsService.AddPoints(userId, -totalPrice);
        }

        public List<StoreItemEntity> GetStoreItems()
        {
            return Cache.StoreItems.Select(kvp => kvp.Value).ToList();
        }

        public async Task<List<StoreItemEntity>> GetUserStoreItems(int userId)
        {
            return await StoreRepository.GetUserStoreItems(userId);
        }

        public async Task UpdateUserItem(int userId, StoreItemEntity item)
        {
            await StoreRepository.UpdateUserItem(userId, item);
        }

        public async Task UpdateUserItems(int userId, List<StoreItemEntity> storeItems)
        {
            await StoreRepository.UpdateUserItems(userId, storeItems);
        }
    }
}