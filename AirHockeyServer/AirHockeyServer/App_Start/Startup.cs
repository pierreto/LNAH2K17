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
using AirHockeyServer.Hubs;
using AirHockeyServer.Events.EventManagers;

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

            GlobalHost.DependencyResolver.Register(
                typeof(GameWaitingRoomHub),
                () => new GameWaitingRoomHub(new GameService(new DataProvider(new RequestsManager(new Connector())))));

            app.MapSignalR("/signalr", new HubConfiguration());
            
        }
    }
}
