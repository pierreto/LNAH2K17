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

        //public event EventHandler<FriendRequestEntity> FriendRequestEvent;
        //public event EventHandler<UserEntity> NewFriendEvent;
        //public event EventHandler<FriendRequestEntity> CanceledFriendRequestEvent;
        //public event EventHandler<UserEntity> RemovedFriendEvent;

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
            /*
            FriendsProxy.On<FriendRequestEntity>("FriendRequestEvent", request =>
            {
                Task.Run(() => FriendsProxy.Invoke("AcceptFriendRequest", request));    
            });
            */

            FriendsProxy.On<UserEntity>("NewFriendEvent", friend =>
            {
                Console.WriteLine("New friend: " + friend.Username);
            });

            FriendsProxy.On<UserEntity>("RemovedFriendEvent", ex_friend =>
            {
                Console.WriteLine("Removed friend: " + ex_friend.Username);
            });
        }

        public async Task<List<UserEntity>> GetAllFriends()
        {
            return await FriendsProxy.Invoke<List<UserEntity>>("GetAllFriends", this.user);
        }

        public async Task<FriendRequestEntity> SendFriendRequest(UserEntity friend)
        {
            var res = await FriendsProxy.Invoke<FriendRequestEntity>("SendFriendRequest", this.user, friend);

            if (res != null)
            {
                Console.WriteLine("user=" + res.Requestor.Username);
                Console.WriteLine("friend=" + res.Friend.Username);
                Console.WriteLine("status=" + res.Status);
            }

            return res;
        }

        public async Task<bool> AcceptFriendRequest(FriendRequestEntity request)
        {
            var res = await FriendsProxy.Invoke<FriendRequestEntity>("AcceptFriendRequest", request);
            return (res != null) ? true : false;
        }

        public async Task<bool> RemoveFriend(UserEntity ex_friend)
        {
            return await FriendsProxy.Invoke<bool>("RemoveFriend", this.user, ex_friend);
        }

        public void Logout()
        {
            // TODO(misg)
        }
    }
}