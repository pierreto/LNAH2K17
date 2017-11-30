using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AirHockeyServer.Core
{
    public class Connector : IConnector
    {
        private HttpClient HttpClient;

        public Connector()
        {
            this.HttpClient = new HttpClient();

            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

        // url need to be updated to Uri
        public async Task<HttpResponseMessage> SendGetRequest(string url)
        {
            HttpResponseMessage response = await this.HttpClient.GetAsync(url);
            return response;
        }

        // url need to be updated to Uri
        public async Task<HttpResponseMessage> SendPostRequest(string url, string body)
        {
            HttpContent httpContent = new StringContent(body);
            HttpResponseMessage response = await this.HttpClient.PostAsync(url, httpContent);
            return response;
        }

    }
}