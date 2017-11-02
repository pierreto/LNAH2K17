using AirHockeyServer.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace AirHockeyServer.Controllers
{
    public class UserController : ApiController
    {

        public UserService UserService { get; }

        public UserController()
        {
            this.UserService = new UserService();
        }

        [HttpGet]
        [Route("api/user/u/{username}")]
        public async Task<HttpResponseMessage> GetUser(string username)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, await UserService.GetUserByUsername(username));
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.NotFound, e);
            }
        }

        [HttpGet]
        [Route("api/user/{id}")]
        public async Task<HttpResponseMessage> GetUser(int id)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, await UserService.GetUserById(id));
            }
            catch (UserException e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.NotFound, e);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("api/user")]
        public async Task<HttpResponseMessage> GetAllUsers()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, await UserService.GetAllUsers());
            }
            catch (UserException e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.NotFound, e);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("api/user")]
        public HttpResponseMessage PostUser()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (UserException e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.Forbidden, e);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}
