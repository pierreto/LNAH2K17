using InterfaceGraphique.CommunicationInterface.RestInterface;
using InterfaceGraphique.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceGraphique.Services
{
    public class PlayerStatsService
    {
        public async Task<PlayerStatsEntity> GetPlayerStats(int userId)
        {
            HttpResponseMessage response = await Program.client.GetAsync("api/stats/" + userId);
            return await HttpResponseParser.ParseResponse<PlayerStatsEntity>(response);
        }

        public async Task<List<Achievement>> GetPlayerAchivements(int userId)
        {
            HttpResponseMessage response = await Program.client.GetAsync("api/achivements/" + userId);
            return await HttpResponseParser.ParseResponse<List<Achievement>>(response);
        }

        public async Task<List<Achievement>> GetAchievements()
        {
            HttpResponseMessage response = await Program.client.GetAsync("api/achivements/");
            return await HttpResponseParser.ParseResponse<List<Achievement>>(response);
        }
    }
}
