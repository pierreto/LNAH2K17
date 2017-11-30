using AirHockeyServer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using AirHockeyServer.Entities;
using Newtonsoft.Json;
using System.Net;

namespace AirHockeyServer.Controllers
{
    public class StatsController : ApiController
    {
        public StatsController(IPlayerStatsService statsService)
        {
            StatsService = statsService;
        }

        public IPlayerStatsService StatsService { get; }

        [HttpGet]
        [Route("api/stats/{id}")]
        public async Task<HttpResponseMessage> GetStats(int id)
        {
            var result = await StatsService.GetPlayerStats(id);
            return CreateHttpResponse(result);
        }

        [HttpGet]
        [Route("api/achivements/{id}")]
        public async Task<HttpResponseMessage> GetAchievements(int id)
        {
            var result = await StatsService.GetAchievements(id);
            return CreateHttpResponse(result);
        }

        [HttpGet]
        [Route("api/achivements")]
        public HttpResponseMessage GetAchievements()
        {
            var result = StatsService.GetAchievements();
            return CreateHttpResponse(result);
        }

        private HttpResponseMessage CreateHttpResponse(Object result)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            if(result != null)
            {
                var stringResult = JsonConvert.SerializeObject(result);
                response.Content = new StringContent(stringResult);
                response.StatusCode = HttpStatusCode.OK;
            }
            else
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }
    }
}