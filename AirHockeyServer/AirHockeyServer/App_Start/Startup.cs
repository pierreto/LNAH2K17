using Microsoft.Owin;
using Owin;
using System.Web.Http;
using Microsoft.Practices.Unity;
using AirHockeyServer.Repositories;
using AirHockeyServer.Core;
using Microsoft.AspNet.SignalR;
using AirHockeyServer.Services.ChatServiceServer;
using AirHockeyServer.Hubs;
using AirHockeyServer.Services;
using Microsoft.Owin.Cors;
using System;
using AirHockeyServer.Services.Interfaces;

[assembly: OwinStartup(typeof(AirHockeyServer.App_Start.Startup))]

namespace AirHockeyServer.App_Start
{
    public class Startup
    {

        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            ConnectionMapper connectionMapper = WebApiApplication.UnityContainer.Resolve<ConnectionMapper>();
            ILoginService loginService = WebApiApplication.UnityContainer.Resolve<ILoginService>();

             GlobalHost.DependencyResolver.Register(
                 typeof(ChatHub),
                 () => new ChatHub(connectionMapper, WebApiApplication.UnityContainer.Resolve<ChannelService>()));

            GlobalHost.DependencyResolver.Register(
                typeof(FriendsHub),
                () => new FriendsHub(WebApiApplication.UnityContainer.Resolve<Services.FriendService>(), connectionMapper, WebApiApplication.UnityContainer.Resolve<GameService>()));

            GlobalHost.DependencyResolver.Register(
                 typeof(GameWaitingRoomHub),
                 () => new GameWaitingRoomHub(WebApiApplication.UnityContainer.Resolve<GameService>(), connectionMapper, WebApiApplication.UnityContainer.Resolve<FriendService>()));

             GlobalHost.DependencyResolver.Register(
                 typeof(TournamentWaitingRoomHub),
                 () => new TournamentWaitingRoomHub(WebApiApplication.UnityContainer.Resolve<TournamentService>(), connectionMapper, WebApiApplication.UnityContainer.Resolve<FriendService>())); 

              GlobalHost.DependencyResolver.Register(
                 typeof(EditionHub),
                 () => new EditionHub(WebApiApplication.UnityContainer.Resolve<EditionService>(), connectionMapper, WebApiApplication.UnityContainer.Resolve<UserService>()));
            // Server
            app.UseCors(CorsOptions.AllowAll);
            GlobalHost.Configuration.DisconnectTimeout = TimeSpan.FromSeconds(6);
            app.MapSignalR("/signalr", new HubConfiguration());
        }
    }
}
