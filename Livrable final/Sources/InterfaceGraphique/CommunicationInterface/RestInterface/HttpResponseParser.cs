using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceGraphique.CommunicationInterface.RestInterface
{
    public class HttpResponseParser
    {
        public async static Task<T> ParseResponse<T>(HttpResponseMessage response)
        {
            try
            {
                string stringContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(stringContent);
            }
            catch
            {
                return default(T);
            }
        }
    }
}
