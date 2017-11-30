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
    public class PlayerStatsService : Service
    {
        public async Task<PlayerStatsEntity> GetPlayerStats(int userId)
        {
            try
            {


                HttpResponseMessage response = await Program.client.GetAsync("api/stats/" + userId);
                return await HttpResponseParser.ParseResponse<PlayerStatsEntity>(response);
            }
            catch (Exception)
            {
                await OnException();
                return null;
            }
        }

        public async Task<List<Achievement>> GetPlayerAchivements(int userId)
        {
            try
            {

                HttpResponseMessage response = await Program.client.GetAsync("api/achivements/" + userId);
                return await HttpResponseParser.ParseResponse<List<Achievement>>(response);
            }
            catch (Exception)
            {
                await OnException();
                return null;
            }
        }

        public async Task<List<Achievement>> GetAchievements()
        {
            try
            {

                HttpResponseMessage response = await Program.client.GetAsync("api/achivements/");
                return await HttpResponseParser.ParseResponse<List<Achievement>>(response);
            }
            catch (Exception)
            {
                await OnException();
                return null;
            }
        }
    }
}
