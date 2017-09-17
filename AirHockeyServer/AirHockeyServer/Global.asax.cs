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

            Register(GlobalConfiguration.Configuration);
            ChatServer server = new ChatServer();
            server.StartListeningAsync();
        }

        public static void Register(HttpConfiguration config)
        {
            var container = new UnityContainer();
            // Repositories
            container.RegisterType<IChannelRepository, ChannelRepository>(new HierarchicalLifetimeManager());

            // Services
            container.RegisterType<IChatService, ChatService>(new HierarchicalLifetimeManager());
            container.RegisterType<IChannelService, ChannelService>(new HierarchicalLifetimeManager());

            //Core
            container.RegisterType<IConnector, Connector>(new HierarchicalLifetimeManager());
            container.RegisterType<IRequestsManager, RequestsManager>(new HierarchicalLifetimeManager());
            container.RegisterType<IDataProvider, DataProvider>(new HierarchicalLifetimeManager());

            config.DependencyResolver = new UnityResolver(container);
            
        }
    }
}
