using AirHockeyServer.Entities;
using AirHockeyServer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace AirHockeyServer.Controllers
{
    public class StoreController : ApiController
    {
        public StoreController(IStoreService storeService)
        {
            StoreService = storeService;
        }

        public IStoreService StoreService { get; }

        [HttpGet]
        [Route("api/store")]
        public HttpResponseMessage GetStoreItems()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, StoreService.GetStoreItems());
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("api/store/{id}")]
        public async Task<HttpResponseMessage> GetUserStoreItems(int id)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, await StoreService.GetUserStoreItems(id));
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("api/store/{id}")]
        public async Task<HttpResponseMessage> AddUserItem(int id, [FromBody] List<StoreItemEntity> storeItems)
        {
            try
            {
                await StoreService.AddUserItems(id, storeItems);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut]
        [Route("api/store/{id}")]
        public async Task<HttpResponseMessage> UpdateUserItem(int id, [FromBody] StoreItemEntity storeItem)
        {
            try
            {
                await StoreService.UpdateUserItem(id, storeItem);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}