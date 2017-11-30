using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceGraphique.CommunicationInterface.RestInterface
{
    public class Repository
    {
        protected HttpClient HttpClient { get; set; }

        public Repository()
        {
            HttpClient = new HttpClient();
            HttpClient.BaseAddress = new Uri("http://localhost:63056/api/");
        }

        protected async Task<HttpResponseMessage> SendGetRequest()
        {
            return await HttpClient.GetAsync("maps");
        }
    }
}
