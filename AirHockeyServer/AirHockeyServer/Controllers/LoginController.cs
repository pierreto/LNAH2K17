using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using AirHockeyServer.Entities;
using AirHockeyServer.Services;

namespace AirHockeyServer.Controllers
{
    public class LoginController : ApiController
    {
        public ILoginService LoginService { get; }

        public LoginController()
        {
            this.LoginService = new LoginService();
        }

        public IChatService ChatService { get; }

        [AcceptVerbs("POST")]
        [Route("api/login")]
        public HttpResponseMessage Login([FromBody]LoginEntity loginEntity)
        {
            try
            {
                this.LoginService.Login(loginEntity);
            }
            catch (LoginException e)
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

        [Route("api/logout")]
        public HttpResponseMessage Logout([FromBody]LoginEntity loginEntity)
        {
            this.LoginService.Logout(loginEntity);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}