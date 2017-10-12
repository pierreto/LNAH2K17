using AirHockeyServer.Entities;
using AirHockeyServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
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

        //
        [HttpGet]
        [Route("api/user/{id}")]
        public async Task<HttpResponseMessage> getUser(int id)
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

        [HttpPost]
        [Route("api/user")]
        public HttpResponseMessage postUser()
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
