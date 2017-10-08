using AirHockeyServer.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace AirHockeyServer.Controllers
{
    public class UserController: ApiController
    {
       
        public UserService UserService { get; }

        public UserController()
        {
            this.UserService = new UserService();
        }

        [HttpGet]
        [Route("api/user/{id}")]
        public async Task<HttpResponseMessage> GetUser(int id)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, await UserService.GetUserById(id));
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

        [HttpGet]
        [Route("api/user")]
        public async Task<HttpResponseMessage> GetAllUsers()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, await UserService.GetAllUsers());
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

        [HttpPost]
        [Route("api/user")]
        public HttpResponseMessage PostUser()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK);

                // return Request.CreateResponse(HttpStatusCode.OK, UserService.PostUser());
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
