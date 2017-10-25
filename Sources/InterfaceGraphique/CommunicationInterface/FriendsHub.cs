using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using InterfaceGraphique.Entities;
using InterfaceGraphique.Game.GameState;
using Microsoft.AspNet.SignalR.Client;

namespace InterfaceGraphique.CommunicationInterface
{
    public class FriendsHub : IBaseHub
    {
        private static IHubProxy FriendsProxy { get; set; }
        private static HubConnection Connection;
        private UserEntity user;

        public event EventHandler<FriendRequestEntity> FriendRequestEvent;
        public event EventHandler<UserEntity> NewFriendEvent;
        public event EventHandler<FriendRequestEntity> CanceledFriendRequestEvent;
        public event EventHandler<UserEntity> RemovedFriendEvent;

       public void InitializeHub(HubConnection connection, string _)
        {
            Connection = connection;
            FriendsProxy = Connection.CreateHubProxy("FriendsHub");
        }

        public async Task InitializeFriendsHub(UserEntity user)
        {
            this.user = user;
            try
            {
                //await FriendsProxy.Invoke("JoinHub", user);
                await FriendsProxy.Invoke("JoinHub");
                //InitializeEvents();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void InitializeEvents()
        {
            FriendsProxy.On<FriendRequestEntity>("FriendRequestEvent", request =>
            {
                Task.Run(() => FriendsProxy.Invoke("AcceptFriendRequest", request));    
            });

            FriendsProxy.On<UserEntity>("NewFriendEvent", friend =>
            {
                Console.WriteLine("New friend: " + friend.Username);
            });
        }

        public async Task<List<UserEntity>> GetAllFriends()
        {
            return await FriendsProxy.Invoke<List<UserEntity>>("GetAllFriends", this.user);
        }

        public void Logout()
        {
            // TODO(misg)
        }
    }
}