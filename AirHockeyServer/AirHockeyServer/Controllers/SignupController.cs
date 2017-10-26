using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using AirHockeyServer.Entities;
using AirHockeyServer.Services;
using System.Threading.Tasks;

namespace AirHockeyServer.Controllers
{
    public class SignupController : ApiController
    {
        public SignupService SignupService { get; }

        public SignupController()
        {
            this.SignupService = new SignupService();
        }

        [HttpPost]
        [Route("api/signup")]
        public async Task<HttpResponseMessage> Signup([FromBody]SignupEntity signupEntity)
        {
            try
            {
                int? id = null;
                id = await this.SignupService.Signup(signupEntity);
                return Request.CreateResponse(HttpStatusCode.OK, id);
            }
            catch (SignupException e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, e.Message);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}