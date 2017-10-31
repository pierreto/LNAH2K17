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

        [HttpGet]
        [Route("api/maps")]
        public async Task<HttpResponseMessage> ReturnAllMaps()
        {
            try
            {
                IEnumerable<MapEntity> maps = await MapService.GetMaps();
                return HttpResponseGenerator.CreateSuccesResponseMessage(HttpStatusCode.OK, maps);
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

        // Hack for returning the db-generated id of a new map.
        // Should only be used ONCE after saving a new map for the first time:
        [HttpPost]
        [Route("api/maps/get_id_new_map")]
        public async Task<HttpResponseMessage> GetMapID([FromBody]MapEntity map)
        {
            try
            {
                int? id = await MapService.GetMapID(map);
                return HttpResponseGenerator.CreateSuccesResponseMessage(HttpStatusCode.OK, id);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("api/maps/get/{id}")]
        public async Task<HttpResponseMessage> GetMapByID(int id)
        {
            try
            {
                MapEntity map = await MapService.GetMap(id);
                return HttpResponseGenerator.CreateSuccesResponseMessage(HttpStatusCode.OK, map);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}