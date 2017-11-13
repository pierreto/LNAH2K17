using InterfaceGraphique.CommunicationInterface.RestInterface;
using InterfaceGraphique.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceGraphique.Services
{
    public class StoreService
    {
        private HttpClient HttpClient { get; set; }

        public StoreService()
        {
            HttpClient = new HttpClient();
        }

        public async Task<bool> BuyElements(List<StoreItemEntity> items)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(items));
            HttpResponseMessage response = await Program.client.PostAsync("api/store/", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<List<StoreItemEntity>> GetStoreItems()
        {
            HttpResponseMessage response = await Program.client.GetAsync("api/store/");
            return await HttpResponseParser.ParseResponse<List<StoreItemEntity>>(response);
        }

        public async Task<List<StoreItemEntity>> GetUserStoreItems(int userId)
        {
            HttpResponseMessage response = await Program.client.GetAsync("api/store/" + userId);
            return await HttpResponseParser.ParseResponse<List<StoreItemEntity>>(response);
        }

        public async Task<bool> UpdateItemEnable(int userId, StoreItemEntity item)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(item));
            HttpResponseMessage response = await Program.client.PutAsync("api/store/" + userId, content);

            return response.IsSuccessStatusCode;
        }
    }
}
