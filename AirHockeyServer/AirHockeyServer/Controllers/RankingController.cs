using AirHockeyServer.Services.Interfaces;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace AirHockeyServer.Controllers
{
    public class RankingController : ApiController
    {
        public IRankingService RankingService { get; }

        public RankingController(IRankingService rankingService)
        {
            this.RankingService = rankingService;
        }

        [HttpGet]
        [Route("api/rankings")]
        public async Task<HttpResponseMessage> GetAllRankings()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, await RankingService.GetAllRankings());
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}