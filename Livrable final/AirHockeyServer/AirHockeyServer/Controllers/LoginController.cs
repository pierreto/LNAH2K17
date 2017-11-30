using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using AirHockeyServer.Entities;
using AirHockeyServer.Services;
using System.Threading.Tasks;
using AirHockeyServer.Services.Interfaces;
using Microsoft.Practices.Unity;

namespace AirHockeyServer.Controllers
{
    public class LoginController : ApiController
    {
        public ILoginService LoginService { get; set; }

        public LoginController(ILoginService loginService)
        {
            this.LoginService = loginService;
        }

        public IChatService ChatService { get; }

        [HttpPost]
        [Route("api/login")]
        public async Task<HttpResponseMessage> Login([FromBody]LoginEntity loginEntity)
        {
            try
            {
                int? id = null;
                id = await this.LoginService.ValidateCredentials(loginEntity);
                return Request.CreateResponse(HttpStatusCode.OK, id);
            }
            catch (LoginException e)
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

        [Route("api/logout")]
        public HttpResponseMessage Logout([FromBody]LoginEntity loginEntity)
        {
            this.LoginService.Logout(loginEntity);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}