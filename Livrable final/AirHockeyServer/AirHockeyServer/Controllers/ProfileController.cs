using AirHockeyServer.Services.Interfaces;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace AirHockeyServer.Controllers
{
    public class ProfileController : ApiController
    {
        public IProfileService ProfileService { get; set; }

        public ProfileController(IProfileService profileService)
        {
            ProfileService = profileService;
        }

        [HttpGet]
        [Route("api/profile/{id}")]
        public async Task<HttpResponseMessage> GetUser(int id)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, await ProfileService.GetProfileById(id));
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}