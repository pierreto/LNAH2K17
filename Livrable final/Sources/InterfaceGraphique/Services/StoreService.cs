using InterfaceGraphique.CommunicationInterface.RestInterface;
using InterfaceGraphique.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using InterfaceGraphique.Controls.WPF.Store;

namespace InterfaceGraphique.Services
{
    public class StoreService : Service
    {
        private HttpClient HttpClient { get; set; }

        public StoreService()
        {
            HttpClient = new HttpClient();
        }

        public async Task<bool> BuyElements(List<StoreItemEntity> items, int userId)
        {
            try
            {

                HttpResponseMessage response = await Program.client.PostAsJsonAsync("api/store/" + userId, items);

                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                await OnException();
                return false;
            }
        }

        public async Task<List<StoreItemEntity>> GetStoreItems()
        {
            try
            {

                HttpResponseMessage response = await Program.client.GetAsync("api/store/");
                return await HttpResponseParser.ParseResponse<List<StoreItemEntity>>(response);
            }
            catch (Exception)
            {
                await OnException();
                return null;
            }
        }

        public async Task<List<StoreItemEntity>> GetUserStoreItems(int userId)
        {
            try
            {

                HttpResponseMessage response = await Program.client.GetAsync("api/store/" + userId);
                return await HttpResponseParser.ParseResponse<List<StoreItemEntity>>(response);
            }
            catch (Exception)
            {
                await OnException();
                return null;
            }
        }

        public async Task<bool> UpdateItemEnable(int userId, StoreItemEntity item)
        {
            try
            {

                HttpResponseMessage response = await Program.client.PutAsJsonAsync("api/store/" + userId + "/" + item.Id, item);

                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                await OnException();
                return false;
            }
        }
    }
}
