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
        public static UnityContainer UnityContainer { get; set; }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Register(GlobalConfiguration.Configuration);
            //ChatServer server = new ChatServer();
            //Task running_server = Task.Run(() => server.StartListeningAsync());
            // We make the server running asynchronously so the REST API can
            // still continue to run:
            //await running_server;
        }

        public static void Register(HttpConfiguration config)
        {
            UnityContainer = new UnityContainer();
            // Repositories
            UnityContainer.RegisterType<IChannelRepository, ChannelRepository>(new HierarchicalLifetimeManager());

            // Services
            UnityContainer.RegisterType<IChatService, ChatService>(new HierarchicalLifetimeManager());
            UnityContainer.RegisterType<IChannelService, ChannelService>(new HierarchicalLifetimeManager());
            UnityContainer.RegisterType<IGameService, GameService>(new HierarchicalLifetimeManager());
            UnityContainer.RegisterType<ITournamentService, TournamentService>(new HierarchicalLifetimeManager());
            UnityContainer.RegisterType<IMapService, MapService>(new HierarchicalLifetimeManager());
            UnityContainer.RegisterType<IEditionService, EditionService>(new ContainerControlledLifetimeManager());


            //Core
            UnityContainer.RegisterType<IConnector, Connector>(new HierarchicalLifetimeManager());
            UnityContainer.RegisterType<IRequestsManager, RequestsManager>(new HierarchicalLifetimeManager());

            config.DependencyResolver = new UnityResolver(UnityContainer);
            GameWaitingRoomEventManager gameWaitingRoomEventManager = UnityContainer.Resolve<GameWaitingRoomEventManager>();
            TournamentWaitingRoomEventManager tournamentWaitingRoomEventManager = UnityContainer.Resolve<TournamentWaitingRoomEventManager>();

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
