using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using Microsoft.Practices.Unity;
using AirHockeyServer.Repositories;
using AirHockeyServer.Services;
using AirHockeyServer.Core;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Optimization;
using Microsoft.AspNet.SignalR;
using AirHockeyServer.Services.ChatServiceServer;
using AirHockeyServer.DatabaseCore;
using MySql.Data.MySqlClient;

[assembly: OwinStartup(typeof(AirHockeyServer.App_Start.Startup))]

namespace AirHockeyServer.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            GlobalHost.DependencyResolver.Register(
                typeof(ChatHub),
                () => new ChatHub(new ChannelService()));

            app.MapSignalR("/signalr", new HubConfiguration());
            //Register(GlobalConfiguration.Configuration);
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

            config.DependencyResolver = new UnityResolver(container);

        }


    }
}
