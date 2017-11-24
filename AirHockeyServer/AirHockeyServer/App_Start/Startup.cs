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
                () => new FriendsHub(WebApiApplication.UnityContainer.Resolve<Services.FriendService>(), WebApiApplication.UnityContainer.Resolve<Hubs.ConnectionMapper>(), WebApiApplication.UnityContainer.Resolve<GameService>()));

            GlobalHost.DependencyResolver.Register(
                 typeof(GameWaitingRoomHub),
                 () => new GameWaitingRoomHub(WebApiApplication.UnityContainer.Resolve<GameService>(), WebApiApplication.UnityContainer.Resolve<Hubs.ConnectionMapper>(), WebApiApplication.UnityContainer.Resolve<FriendService>()));

             GlobalHost.DependencyResolver.Register(
                 typeof(TournamentWaitingRoomHub),
                 () => new TournamentWaitingRoomHub(WebApiApplication.UnityContainer.Resolve<TournamentService>(), WebApiApplication.UnityContainer.Resolve<Hubs.ConnectionMapper>(), WebApiApplication.UnityContainer.Resolve<FriendService>())); 

              GlobalHost.DependencyResolver.Register(
                 typeof(EditionHub),
                 () => new EditionHub(WebApiApplication.UnityContainer.Resolve<EditionService>(), WebApiApplication.UnityContainer.Resolve<Hubs.ConnectionMapper>(), WebApiApplication.UnityContainer.Resolve<UserService>())); 

            app.MapSignalR("/signalr", new HubConfiguration());
        }
    }
}
