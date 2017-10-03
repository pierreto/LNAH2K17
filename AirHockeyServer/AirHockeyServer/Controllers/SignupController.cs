using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using AirHockeyServer.Entities;
using AirHockeyServer.Services;

namespace AirHockeyServer.Controllers
{
    public class SignupController : ApiController
    {
        public ISignupService signupService { get; }

        public SignupController()
        {
            this.signupService = new SignupService();
        }

        //Comment on sait si c'est un post?
        [Route("api/signup")]
        public HttpResponseMessage Signup([FromBody]SignupMessage message)
        {
            try
            {
                this.signupService.Signup(message);
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