﻿using System;
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
        public ILoginService loginService { get; }

        public LoginController()
        {
            this.loginService = new LoginService();
        }

        public IChatService ChatService { get; }

        [Route("api/login")]
        public HttpResponseMessage Post([FromBody]LoginMessage message)
        {
            try
            {
                this.loginService.login(message);
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

    }
}