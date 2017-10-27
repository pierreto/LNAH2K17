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
        private string nameSavedMap;
        private bool savedOnline;
        public async Task<List<MapEntity>> GetOnlineMap()
        {
            HttpResponseMessage response = await Program.client.GetAsync("api/maps");

            return  await HttpResponseParser.ParseResponse<List<MapEntity>>(response);
  
        }

    }
}
