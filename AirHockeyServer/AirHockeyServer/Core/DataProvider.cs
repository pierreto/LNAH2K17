using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace AirHockeyServer.Core
{
    public class DataProvider : IDataProvider
    {
        public DataProvider(IRequestsManager requestsManager)
        {
            // TODO : CONNECT TO BD
            RequestsManager = requestsManager;
        }

        public IRequestsManager RequestsManager { get; }

        public async Task<List<T>> GetEntities<T>(string url)
        {
            var result = await RequestsManager.SendGetRequest(url);
            return await ParseResponse<List<T>>(result);
        }

        public async Task<T> GetEntity<T>(string url)
        {
            var result = await RequestsManager.SendGetRequest(url);
            return await ParseResponse<T>(result);
        }

        private async Task<T> ParseResponse<T>(HttpResponseMessage responseMessage)
        {
            try
            {
                return await responseMessage.Content.ReadAsAsync<T>();
            }
            catch(Exception)
            {
                return default(T);
            }
        }
    }
}