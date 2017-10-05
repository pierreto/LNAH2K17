using AirHockeyServer.Entities;
using AirHockeyServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
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

        //Comment on sait si c'est un post?
        [HttpGet]
        [Route("api/user/{id}")]
        public UserEntity getUser(int id)
        {
            try
            {
                return(this.UserService.GetUserById(id));
            }
            catch (SignupException e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return null;
                //return Request.CreateResponse(HttpStatusCode.Forbidden);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return null;
                //return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            //return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("api/user")]
        public HttpResponseMessage postUser()
        {
            try
            {
                //this.UserService.PostUser();
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
