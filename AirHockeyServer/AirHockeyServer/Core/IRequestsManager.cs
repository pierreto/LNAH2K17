using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AirHockeyServer.Core
{
    public interface IRequestsManager
    {
        Task<HttpResponseMessage> SendGetRequest(string recipient);

        Task<HttpResponseMessage> SendPostRequest(string recipient, object body);
    }
}
