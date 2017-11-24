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

        public event Action<GameRequestEntity> GameRequestEvent;
        public event Action<GameRequestEntity> DeclinedGameRequestEvent;
        public event Action<GameRequestEntity> AcceptedGameRequestEvent;
        public event Action<GameRequestEntity> GameRequestCanceledEvent;
        public event Action<int> NewFriendHasConnectedEvent;
        public event Action<int> NewFriendHasDisconnectedEvent;

        public void InitializeHub(HubConnection connection)
        {
            Connection = connection;
            FriendsProxy = Connection.CreateHubProxy("FriendsHub");
        }

        public async Task InitializeFriendsHub()
        {
            this.user = User.Instance.UserEntity;
            await FriendsProxy.Invoke("JoinHub", this.user);
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

            FriendsProxy.On<GameRequestEntity>("GameRequest", request =>
            {
                GameRequestEvent?.Invoke(request);
            });

            FriendsProxy.On<GameRequestEntity>("DeclinedGameRequest", request =>
            {
                DeclinedGameRequestEvent?.Invoke(request);
            });

            FriendsProxy.On<GameRequestEntity>("AcceptedGameRequest", request =>
            {
                AcceptedGameRequestEvent?.Invoke(request);
            });

            FriendsProxy.On<int>("NewFriendHasConnectedEvent", request =>
            {
                NewFriendHasConnectedEvent?.Invoke(request);
            });

            FriendsProxy.On<int>("NewFriendHasDisconnectedEvent", request =>
            {
                NewFriendHasDisconnectedEvent?.Invoke(request);
            });

            FriendsProxy.On<GameRequestEntity>("GameRequestCanceled", request => { GameRequestCanceledEvent?.Invoke(request); });
        }

        public async Task<bool> FriendIsAvailable(int recipientId)
        {
            return await FriendsProxy.Invoke<bool>("FriendIsAvailable", recipientId);
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

        public async Task AcceptGameRequest(GameRequestEntity gameRequest)
        {
            await FriendsProxy.Invoke("AcceptGameRequest", gameRequest);
        }

        public async Task DeclineGameRequest(GameRequestEntity gameRequest)
        {
            await FriendsProxy.Invoke("DeclineGameRequest", gameRequest);
        }

        public async Task<bool> SendGameRequest(GameRequestEntity gameRequest)
        {
            return await FriendsProxy.Invoke<bool>("SendGameRequest", gameRequest);
        }

        public async Task<bool> CancelGameRequest(GameRequestEntity gameRequest)
        {
            return await FriendsProxy.Invoke<bool>("CancelGameRequest", gameRequest);
        }

        public async Task Logout()
        {
            await FriendsProxy.Invoke("Logout", User.Instance.UserEntity);
        }

        public async Task LeaveRoom()
        {
            // do nothing
            return;
        }
    }
}