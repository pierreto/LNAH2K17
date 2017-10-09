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
    public class LoginController : ApiController
    {
        public ILoginService LoginService { get; }

        public LoginController()
        {
            this.LoginService = new LoginService();
        }

        public IChatService ChatService { get; }

        [HttpPost]
        [Route("api/login")]
        public async Task<HttpResponseMessage> Login([FromBody]LoginEntity loginEntity)
        {
            try
            {
                if (await this.LoginService.ValidateCredentials(loginEntity))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, true);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, false);
                }
                
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
        }

        [Route("api/logout")]
        public HttpResponseMessage Logout([FromBody]LoginEntity loginEntity)
        {
            this.LoginService.Logout(loginEntity);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}