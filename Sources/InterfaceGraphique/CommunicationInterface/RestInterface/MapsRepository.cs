using InterfaceGraphique.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceGraphique.CommunicationInterface.RestInterface
{
    public class MapsRepository : Repository
    {
        public async Task<List<MapEntity>> GetMaps()
        {
            HttpResponseMessage response = await SendGetRequest();
            return await HttpResponseParser.ParseResponse<List<MapEntity>>(response);
        }
        
    }
}
