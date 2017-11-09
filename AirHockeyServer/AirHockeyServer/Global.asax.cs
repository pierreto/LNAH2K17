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
using AirHockeyServer.Repositories.Interfaces;
using AirHockeyServer.Manager;
using AirHockeyServer.DatabaseCore;
using AirHockeyServer.Mapping;
using AirHockeyServer.Services.Interfaces;
using AirHockeyServer.Hubs;

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

            //Register(GlobalConfiguration.Configuration);
            //ChatServer server = new ChatServer();
            //Task running_server = Task.Run(() => server.StartListeningAsync());
            // We make the server running asynchronously so the REST API can
            // still continue to run:
            //await running_server;

            Register(GlobalConfiguration.Configuration);

            Cache cache = new Cache();
        }

        public static async void Register(HttpConfiguration config)
        {
            UnityContainer = new UnityContainer();

            // EventManager
            UnityContainer.RegisterType<GameWaitingRoomEventManager>(new HierarchicalLifetimeManager());
            UnityContainer.RegisterType<TournamentWaitingRoomEventManager>(new HierarchicalLifetimeManager());

            // Repositories
            UnityContainer.RegisterType<IChannelRepository, ChannelRepository>(new HierarchicalLifetimeManager());
            UnityContainer.RegisterType<IMapRepository, MapRepository>(new HierarchicalLifetimeManager());
            UnityContainer.RegisterType<IGameRepository, GameRepository>(new HierarchicalLifetimeManager());
            UnityContainer.RegisterType<IChannelRepository, ChannelRepository>(new HierarchicalLifetimeManager());
            UnityContainer.RegisterType<ILoginRepository, LoginRepository>(new HierarchicalLifetimeManager());
            UnityContainer.RegisterType<IPasswordRepository, PasswordRepository>(new HierarchicalLifetimeManager());
            UnityContainer.RegisterType<IPlayerStatsRepository, PlayerStatsRepository>(new HierarchicalLifetimeManager());
            UnityContainer.RegisterType<IUserRepository, UserRepository>(new HierarchicalLifetimeManager());
            UnityContainer.RegisterType<ITournamentRepository, TournamentRepository>(new HierarchicalLifetimeManager());
            UnityContainer.RegisterType<IFriendRequestRepository, FriendRequestRepository>(new HierarchicalLifetimeManager());

            // Services
            UnityContainer.RegisterType<IChatService, ChatService>(new HierarchicalLifetimeManager());
            UnityContainer.RegisterType<IChannelService, ChannelService>(new HierarchicalLifetimeManager());
            UnityContainer.RegisterType<IGameService, GameService>(new HierarchicalLifetimeManager());
            UnityContainer.RegisterType<ITournamentService, TournamentService>(new HierarchicalLifetimeManager());
            UnityContainer.RegisterType<IMapService, MapService>(new ContainerControlledLifetimeManager());
            UnityContainer.RegisterType<IEditionService, EditionService>(new ContainerControlledLifetimeManager());
            UnityContainer.RegisterType<ILoginService, LoginService>(new ContainerControlledLifetimeManager());
            UnityContainer.RegisterType<IPasswordService, PasswordService>(new ContainerControlledLifetimeManager());
            UnityContainer.RegisterType<IPlayerStatsService, PlayerStatsService>(new ContainerControlledLifetimeManager());
            UnityContainer.RegisterType<ISignupService, SignupService>(new ContainerControlledLifetimeManager());
            UnityContainer.RegisterType<ITournamentService, TournamentService>(new ContainerControlledLifetimeManager());
            UnityContainer.RegisterType<ILoginService, LoginService>(new ContainerControlledLifetimeManager());
            UnityContainer.RegisterType<IUserService, UserService>(new ContainerControlledLifetimeManager());
            UnityContainer.RegisterType<IFriendService, FriendService>(new ContainerControlledLifetimeManager());
            UnityContainer.RegisterType<IAchievementInfoService, AchievementInfoService>(new ContainerControlledLifetimeManager());
            
            // Core
            UnityContainer.RegisterType<IConnector, Connector>(new ContainerControlledLifetimeManager());
            UnityContainer.RegisterType<IRequestsManager, RequestsManager>(new ContainerControlledLifetimeManager());
            UnityContainer.RegisterType<DataProvider>(new ContainerControlledLifetimeManager());

            // Mapping
            UnityContainer.RegisterType<MapperManager>(new ContainerControlledLifetimeManager());
            UnityContainer.RegisterType<Hubs.ConnectionMapper>(new ContainerControlledLifetimeManager());

            // Managers
            UnityContainer.RegisterType<IGameManager, GameManager>(new ContainerControlledLifetimeManager());
            UnityContainer.RegisterType<ITournamentManager, TournamentManager>(new ContainerControlledLifetimeManager());

            config.DependencyResolver = new UnityResolver(UnityContainer);
            GameWaitingRoomEventManager gameWaitingRoomEventManager = UnityContainer.Resolve<GameWaitingRoomEventManager>();
            TournamentWaitingRoomEventManager tournamentWaitingRoomEventManager = UnityContainer.Resolve<TournamentWaitingRoomEventManager>();
        }
    }
}
