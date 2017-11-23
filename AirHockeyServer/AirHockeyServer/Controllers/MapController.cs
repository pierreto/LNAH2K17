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
using AirHockeyServer.Hubs;
using Microsoft.Practices.ObjectBuilder2;
using AirHockeyServer.Services.Interfaces;

namespace AirHockeyServer.Controllers
{
    public class MapController : ApiController
    {

        private IEditionService editionService;
        public MapController(IMapService mapService, IEditionService editionService)
        {
            MapService = mapService;
            this.editionService = editionService;

        }

        public IMapService MapService { get; }

        [HttpGet]
        [Route("api/maps")]
        public async Task<HttpResponseMessage> ReturnAllMaps()
        {
            try
            {
                IEnumerable<MapEntity> maps = await MapService.GetMaps();
                maps.ForEach(action =>
                {
                    string gameId = EditionHub.ObtainEditionGroupIdentifier((int)action.Id);
                    if (editionService.UsersPerGame.ContainsKey(gameId))
                    {
                        action.CurrentNumberOfPlayer = editionService.UsersPerGame[gameId].users.Count;

                    }
                    else
                    {
                        action.CurrentNumberOfPlayer = 0;
                    }
                });
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
            if (map.Id == null)
            {
                int? mapId = await MapService.SaveNewMap(map);
                return HttpResponseGenerator.CreateSuccesResponseMessage(HttpStatusCode.OK, mapId);
            }
            else
            {
                bool saved = await MapService.SaveMap(map);
                return saved ? Request.CreateResponse(HttpStatusCode.OK) :
                    Request.CreateResponse(HttpStatusCode.InternalServerError);
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

        [HttpGet]
        [Route("api/maps/remove/{id}")]
        public async Task<HttpResponseMessage> RemoveMap(int id)
        {
            try
            {
                bool result = await MapService.RemoveMap(id);
                return HttpResponseGenerator.CreateSuccesResponseMessage(HttpStatusCode.OK, result);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("api/maps/sync")]
        public async Task<HttpResponseMessage> SyncMap([FromBody]MapEntity map)
        {
            try
            {
                bool hasBeenModified = await MapService.SyncMap(map);
                return HttpResponseGenerator.CreateSuccesResponseMessage(HttpStatusCode.OK, hasBeenModified);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}