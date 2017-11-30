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
    public class MapService : Service
    {
        public async Task<List<MapEntity>> GetMaps()
        {
            try
            {
                HttpResponseMessage response = await Program.client.GetAsync("api/maps");
                return await HttpResponseParser.ParseResponse<List<MapEntity>>(response);
            }
            catch (Exception)
            {
                await OnException();
                return null;
            }
        }

        public async Task<MapEntity> GetMap(int id)
        {
            try
            {
                HttpResponseMessage response = await Program.client.GetAsync("api/maps/get/" + id.ToString());
                return await HttpResponseParser.ParseResponse<MapEntity>(response);
            }
            catch (Exception)
            {
                await OnException();
                return null;
            }
        }

        public async Task<int?> SaveNewMap(MapEntity map)
        {
            try
            {
                HttpResponseMessage response = await Program.client.PostAsJsonAsync("api/maps/save", map);
                return await HttpResponseParser.ParseResponse<int?>(response);
            }
            catch (Exception)
            {
                await OnException();
                return null;
            }
        }

        public async Task<bool> SaveMap(MapEntity map)
        {
            try
            {
                HttpResponseMessage response = await Program.client.PostAsJsonAsync("api/maps/save", map);
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                await OnException();
                return false;
            }
        }

        public async Task<bool> RemoveMap(int id)
        {
            try
            {
                HttpResponseMessage response = await Program.client.GetAsync("api/maps/remove/" + id.ToString());
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