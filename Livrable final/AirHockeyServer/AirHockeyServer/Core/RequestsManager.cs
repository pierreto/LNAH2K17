using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace AirHockeyServer.Core
{
    public class RequestsManager : IRequestsManager
    {

        public RequestsManager(IConnector connector)
        {
            Connector = connector;
        }

        public IConnector Connector { get; }

        public async Task<HttpResponseMessage> SendGetRequest(string recipient)
        {
            var result = await this.Connector.SendGetRequest(recipient);

            return result;
            // TODO : Queue for request not sent
        }

        public async Task<HttpResponseMessage> SendPostRequest(string recipient, object body)
        {
            var objectString = JsonConvert.SerializeObject(body);
            var result = await this.Connector.SendPostRequest(recipient, objectString);

            // TODO : Queue for request not sent

            return result;
        }
        
    }
}