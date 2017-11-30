using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AirHockeyServer.Core
{
    public interface IConnector
    {
        Task<HttpResponseMessage> SendGetRequest(string url);

        Task<HttpResponseMessage> SendPostRequest(string url, string body);
    }
}
