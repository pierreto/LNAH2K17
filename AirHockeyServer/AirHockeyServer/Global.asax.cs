using AirHockeyServer.App_Start;
using AirHockeyServer.Core;
using AirHockeyServer.Repositories;
using AirHockeyServer.Services;
using AirHockeyServer.Services.ChatServiceServer;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;
using System.Threading.Tasks;
using AirHockeyServer.Events.EventManagers;
using Microsoft.AspNet.SignalR;
using AirHockeyServer.Entities;

namespace AirHockeyServer
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
            GameWaitingRoomEventManager gameWaitingRoomEventManager = new GameWaitingRoomEventManager();
            TournamentWaitingRoomEventManager tournamentWaitingRoomEventManager = new TournamentWaitingRoomEventManager();
            Cache cache = new Cache();
        }
        
        
        private bool IsWebApiRequest()
        {
            return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.StartsWith("~/api");
        }

        protected void Application_PostAuthorizeRequest()
        {

            if (IsWebApiRequest())
            {
                HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
            }
        }
        /* --- */
    }
}
