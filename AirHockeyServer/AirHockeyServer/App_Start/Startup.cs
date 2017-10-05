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
                () => new ChatHub(new ChannelService(new DataProvider(new RequestsManager(new Connector())))));

            app.MapSignalR("/signalr", new HubConfiguration());
            /*
            var dbCon = DatabaseConnector.Instance();
            dbCon.DatabaseName = "log3900";
            if(dbCon.IsConnect())
            {
                string query = "SELECT id_user, username FROM test_users";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string id = reader.GetString(0);
                    string username = reader.GetString(1);
                    System.Diagnostics.Debug.WriteLine(id);
                    System.Diagnostics.Debug.WriteLine(username);
                }
            }*/
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
            container.RegisterType<IDataProvider, DataProvider>(new HierarchicalLifetimeManager());

            config.DependencyResolver = new UnityResolver(container);

        }


    }
}
