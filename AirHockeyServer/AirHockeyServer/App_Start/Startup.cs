using Microsoft.Owin;
using Owin;
using System.Web.Http;
using Microsoft.Practices.Unity;
using AirHockeyServer.Repositories;
using AirHockeyServer.Services;
using AirHockeyServer.Core;
using Microsoft.AspNet.SignalR;
using AirHockeyServer.Services.ChatServiceServer;
using AirHockeyServer.Hubs;
using System;

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
                () => new ChatHub());

            GlobalHost.DependencyResolver.Register(
                typeof(GameWaitingRoomHub),
                () => new GameWaitingRoomHub());

            GlobalHost.DependencyResolver.Register(
                typeof(TournamentWaitingRoomHub),
                () => new TournamentWaitingRoomHub());

            app.MapSignalR("/signalr", new HubConfiguration());
        }
    }
}
