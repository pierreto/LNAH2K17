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

            //Register(GlobalConfiguration.Configuration);

            GameWaitingRoomEventManager gameWaitingRoomEventManager = new GameWaitingRoomEventManager();
            TournamentWaitingRoomEventManager tournamentWaitingRoomEventManager = new TournamentWaitingRoomEventManager();
        }

        public static void Register(HttpConfiguration config)
        {
            //var container = new UnityContainer();

            //// Repositories
            //container.RegisterType<IChannelRepository, ChannelRepository>(new HierarchicalLifetimeManager());
            //container.RegisterType<IPlayerStatsRepository, PlayerStatsRepository>(new HierarchicalLifetimeManager());

            //// Services
            //container.RegisterType<IChatService, ChatService>(new HierarchicalLifetimeManager());
            //container.RegisterType<IChannelService, ChannelService>(new HierarchicalLifetimeManager());
            //container.RegisterType<IGameService, GameService>(new HierarchicalLifetimeManager());
            //container.RegisterType<ITournamentService, TournamentService>(new HierarchicalLifetimeManager());
            //container.RegisterType<IMapService, MapService>(new HierarchicalLifetimeManager());
            //container.RegisterType<IPlayerStatsService, PlayerStatsService>(new HierarchicalLifetimeManager());

            ////Core
            //container.RegisterType<IConnector, Connector>(new HierarchicalLifetimeManager());
            //container.RegisterType<IRequestsManager, RequestsManager>(new HierarchicalLifetimeManager());

            //GameManager gameManager = new GameManager(container.Resolve<IPlayerStatsService>());
            //container.RegisterInstance<GameManager>(gameManager);

            

            //TournamentManager tournamentManager = new TournamentManager(container.Resolve<IPlayerStatsService>(), container.Resolve<GameManager>());

            //config.DependencyResolver = new UnityResolver(container);


            //container.RegisterInstance<TournamentManager>(tournamentManager);

        }

        /*
         * TODO(Michael): removing that?
         * ---
         */
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
