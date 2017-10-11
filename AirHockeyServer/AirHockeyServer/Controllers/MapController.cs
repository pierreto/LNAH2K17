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
        public MapController(IMapService mapService)
        {
            MapService = mapService;
        }

        public IMapService MapService { get; }

        [Route("api/maps")]
        public HttpResponseMessage Get()
        {
            IEnumerable<MapEntity> maps = MapService.GetMaps();
            if(maps != null)
            {
                return HttpResponseGenerator.CreateSuccesResponseMessage(HttpStatusCode.OK, maps);
            }
            else
            {
                return HttpResponseGenerator.CreateErrorResponseMessage(HttpStatusCode.NotFound);
            }
        }

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