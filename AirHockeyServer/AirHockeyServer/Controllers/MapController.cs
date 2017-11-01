using AirHockeyServer.Entities;
using AirHockeyServer.Services;
using AirHockeyServer.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Threading.Tasks;

namespace AirHockeyServer.Controllers
{
    public class MapController : ApiController 
    {
        public MapController()
        {
            MapService = new MapService();
        }

        public IMapService MapService { get; }

        [HttpGet]
        [Route("api/maps")]
        public async Task<HttpResponseMessage> ReturnAllMaps()
        {
            try
            {
                IEnumerable<MapEntity> maps = await MapService.GetMaps();
                var response = HttpResponseGenerator.CreateSuccesResponseMessage(HttpStatusCode.OK, maps);
                return response;
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("api/maps/save")]
        public async Task<HttpResponseMessage> SaveMap([FromBody]MapEntity map)
        {
            try
            {
                await MapService.SaveMap(map);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [Route("api/maps/get/{name}")]
        public async Task<HttpResponseMessage> GetMapByName(string name)
        {
            MapEntity map = await MapService.GetMapByName("misg", name);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}