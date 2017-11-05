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
                 () => new ChatHub(WebApiApplication.UnityContainer.Resolve<ChannelService>()));

            GlobalHost.DependencyResolver.Register(
                typeof(FriendsHub),
                () => new FriendsHub(WebApiApplication.UnityContainer.Resolve<FriendService>()));

            GlobalHost.DependencyResolver.Register(
                 typeof(GameWaitingRoomHub),
                 () => new GameWaitingRoomHub(WebApiApplication.UnityContainer.Resolve<GameService>()));

             GlobalHost.DependencyResolver.Register(
                 typeof(TournamentWaitingRoomHub),
                 () => new TournamentWaitingRoomHub(WebApiApplication.UnityContainer.Resolve<TournamentService>())); 

              GlobalHost.DependencyResolver.Register(
                 typeof(EditionHub),
                 () => new EditionHub(WebApiApplication.UnityContainer.Resolve<EditionService>())); 

            app.MapSignalR("/signalr", new HubConfiguration());
        }
    }
}
