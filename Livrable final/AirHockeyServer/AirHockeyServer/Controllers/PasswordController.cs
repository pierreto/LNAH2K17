using AirHockeyServer.Services;
using AirHockeyServer.Services.Interfaces;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace AirHockeyServer.Controllers
{
    public class PasswordController : ApiController
    {

        public IPasswordService PasswordService { get; }

        public PasswordController(IPasswordService passwordService)
        {
            this.PasswordService = passwordService;
        }

        [HttpGet]
        [Route("api/password/{id}")]
        public async Task<HttpResponseMessage> GetUser(int id)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, await PasswordService.GetPasswordById(id));
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
        }
    }
}
