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
                await this.SignupService.Signup(signupEntity);
            }
            catch (SignupException e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}