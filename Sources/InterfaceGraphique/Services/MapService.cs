using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InterfaceGraphique.CommunicationInterface.RestInterface;
using InterfaceGraphique.Entities;

namespace InterfaceGraphique.Services
{
    public class MapService
    {
        public async Task<List<MapEntity>> GetMaps()
        {
            HttpResponseMessage response = await Program.client.GetAsync("api/maps");
            return await HttpResponseParser.ParseResponse<List<MapEntity>>(response);
        }

        public async Task<MapEntity> GetMap(int id)
        {
            HttpResponseMessage response = await Program.client.GetAsync("api/maps/get/" + id.ToString());
            return await HttpResponseParser.ParseResponse<MapEntity>(response);
        }

        public async Task<int?> SaveNewMap(MapEntity map)
        {
            HttpResponseMessage response = await Program.client.PostAsJsonAsync("api/maps/save", map);
            return await HttpResponseParser.ParseResponse<int?>(response);
        }

        public async Task<bool> SaveMap(MapEntity map)
        {
            HttpResponseMessage response = await Program.client.PostAsJsonAsync("api/maps/save", map);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveMap(int id)
        {
            HttpResponseMessage response = await Program.client.GetAsync("api/maps/remove/" + id.ToString());
            return response.IsSuccessStatusCode;
        }
    }
}