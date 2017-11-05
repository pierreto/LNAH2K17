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

        public event Action<FriendRequestEntity> FriendRequestEvent;
        public event Action<UserEntity> NewFriendEvent;
        public event Action<UserEntity> RemovedFriendEvent;
        public event Action<FriendRequestEntity> CanceledFriendRequestEvent;

       public void InitializeHub(HubConnection connection)
        {
            Connection = connection;
            FriendsProxy = Connection.CreateHubProxy("FriendsHub");
        }

        public async Task InitializeFriendsHub(UserEntity user)
        {
            this.user = user;
            await FriendsProxy.Invoke("JoinHub", user);
            InitializeEvents();
        }

        private void InitializeEvents()
        {
            FriendsProxy.On<FriendRequestEntity>("FriendRequestEvent", friendRequest =>
            {
                FriendRequestEvent?.Invoke(friendRequest);
            });

            FriendsProxy.On<UserEntity>("NewFriendEvent", friend =>
            {
                NewFriendEvent?.Invoke(friend);
            });

            FriendsProxy.On<UserEntity>("RemovedFriendEvent", ex_friend =>
            {
                RemovedFriendEvent?.Invoke(ex_friend);
            });

            FriendsProxy.On<FriendRequestEntity>("CanceledFriendRequestEvent", request =>
            {
                CanceledFriendRequestEvent?.Invoke(request);
            });
        }

        public async Task<List<UserEntity>> GetAllFriends()
        {
            return await FriendsProxy.Invoke<List<UserEntity>>("GetAllFriends", this.user);
        }

        public async Task<List<FriendRequestEntity>> GetAllPendingRequests()
        {
            return await FriendsProxy.Invoke<List<FriendRequestEntity>>("GetAllPendingRequests", this.user);
        }

        public async Task<FriendRequestEntity> SendFriendRequest(UserEntity friend)
        {
            return await FriendsProxy.Invoke<FriendRequestEntity>("SendFriendRequest", this.user, friend);
        }

        public async Task<bool> AcceptFriendRequest(FriendRequestEntity request)
        {
            var res = await FriendsProxy.Invoke<FriendRequestEntity>("AcceptFriendRequest", request);
            return (res != null) ? true : false;
        }

        public async Task<bool> RefuseFriendRequest(FriendRequestEntity request)
        {
            var res = await FriendsProxy.Invoke<FriendRequestEntity>("RefuseFriendRequest", request);
            return (res != null) ? true : false;
        }
        public async Task<bool> RemoveFriend(UserEntity ex_friend)
        {
            return await FriendsProxy.Invoke<bool>("RemoveFriend", this.user, ex_friend);
        }

        public async Task Logout()
        {
            // TODO(misg)
        }
    }
}