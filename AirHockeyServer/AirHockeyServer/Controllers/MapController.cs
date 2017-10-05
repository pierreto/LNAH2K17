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
        public HttpResponseMessage SaveMap([FromBody]MapEntity message)
        {
            System.Diagnostics.Debug.WriteLine(message.Creator.Name);
            System.Diagnostics.Debug.WriteLine(message.MapName);
            System.Diagnostics.Debug.WriteLine(message.LastBackup);
            System.Diagnostics.Debug.WriteLine(message.Json);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}