using System;
using System.Collections.Generic;
using Microsoft.AspNet.SignalR;
using System.Linq;
using System.Web;
using AirHockeyServer.Services;
using AirHockeyServer.Entities;
using System.Threading.Tasks;
using AirHockeyServer.Events;
using AirHockeyServer.Core;

namespace AirHockeyServer.Hubs
{
    public class FriendsHub : BaseHub 
    {
        protected FriendService FriendService { get; }
        public ConnectionMapper ConnectionMapper { get; }
        public GameService GameService { get; }

        public FriendsHub(FriendService friendService, ConnectionMapper connectionMapper, GameService gameService) 
            :base(connectionMapper)
        {
            FriendService = friendService;
            ConnectionMapper = connectionMapper;
            GameService = gameService;
        }

        public void JoinHub(UserEntity user)
        {
            ConnectionMapper.AddConnection(user.Id, Context.ConnectionId);
            FriendService.NewUserConnected(user);
            Clients.AllExcept(this.Context.ConnectionId).NewFriendHasConnectedEvent(user.Id);

        }

        public async Task<List<UserEntity>> GetAllFriends(UserEntity user)
        {
            List<UserEntity> allFriend = await FriendService.GetAllFriends(user);
            return SetIsConnected(allFriend);
        }
         
        private List<UserEntity> SetIsConnected(List<UserEntity> users)
        {
            users.ForEach((friend =>
            {
                if (this.FriendService.UsersIdConnected.Exists(x => friend.Id == x.Id))
                {
                    friend.IsConnected = true;
                }
            }));
            return users;
        }
        private UserEntity SetIsConnected(UserEntity user)
        {

            if (user !=null && this.FriendService.UsersIdConnected.Exists(x => user.Id == x.Id))
            {
                user.IsConnected = true;
            }
            return user;
        }

        public async Task<List<FriendRequestEntity>> GetAllPendingRequests(UserEntity user)
        {
            var requests = await FriendService.GetAllPendingRequests(user);
            return requests;
        }

        public async Task<FriendRequestEntity> SendFriendRequest(UserEntity user, UserEntity friend)
        {
            var friendRequest = await FriendService.SendFriendRequest(user, friend);

            // On notifie l'utilisateur dont on veut être l'ami de la demande:
            string friendConnection = ConnectionMapper.GetConnection(friend.Id);
            if (friendRequest != null && friendConnection.Length > 0) 
            {
                SetIsConnected(friendRequest.Friend);
                SetIsConnected(friendRequest.Requestor);
                Clients.Client(friendConnection).FriendRequestEvent(friendRequest);
            }

            return friendRequest;
        }
        
        public async Task<FriendRequestEntity> AcceptFriendRequest(FriendRequestEntity request)
        {
            FriendRequestEntity relation = await FriendService.AcceptFriendRequest(request);

            // Si la demande d'ami a été acceptée avec succès, on notifie les deux
            // nouveaux amis (requestor et friend) :
            string friendConnection = ConnectionMapper.GetConnection(request.Requestor.Id);
            string myConnection = ConnectionMapper.GetConnection(request.Friend.Id);
            if (relation != null)
            {
                SetIsConnected(request.Friend);
                SetIsConnected(request.Requestor);
                if (friendConnection.Length > 0)
                {
                    Clients.Client(friendConnection).NewFriendEvent(request.Friend);
                }
              
                Clients.Client(myConnection).NewFriendEvent(request.Requestor);

            }

            return relation;
        }

        public async Task<FriendRequestEntity> RefuseFriendRequest(FriendRequestEntity request)
        {
            SetIsConnected(request.Friend);
            SetIsConnected(request.Requestor);
            return await FriendService.RefuseFriendRequest(request);
            // TODO ? envoyer un event pour pouvoir ajouter a la liste d'amis a ajouter quand on refuse qqn
        }
        
        public async Task<bool> CancelFriendRequest(FriendRequestEntity request)
        {
            var canceled_request = await FriendService.CancelFriendRequest(request);

            // Si la demande a été annulée avec succès, on notifie le client à qui on a envoyé
            // la demande :
            string friendConnection = ConnectionMapper.GetConnection(request.Friend.Id);
            if (canceled_request && friendConnection.Length > 0)
            {
                SetIsConnected(request.Friend);
                SetIsConnected(request.Requestor);
                Clients.Client(friendConnection).CanceledFriendRequestEvent(request);
            }

            return canceled_request;
        }

        public async Task<bool> RemoveFriend(UserEntity user, UserEntity ex_friend)
        {
            var removed_friend = await FriendService.RemoveFriend(user, ex_friend);

            // Si la relation a été supprimée avec succès, on notifie chaque utilisateur de la
            // perte de son ami :
            string ex_friendConnection = ConnectionMapper.GetConnection(ex_friend.Id);
            if (removed_friend)
            {
                Clients.Client(ConnectionMapper.GetConnection(user.Id)).RemovedFriendEvent(ex_friend);

                if (ex_friendConnection.Length > 0)
                {
                    SetIsConnected(user);
                    Clients.Client(ex_friendConnection).RemovedFriendEvent(user);
                }
            }

            return removed_friend;
        }

        public async Task<bool> SendGameRequest(GameRequestEntity gameRequest)
        {

            string friendConnection = ConnectionMapper.GetConnection(gameRequest.Recipient.Id);

            if (!String.IsNullOrEmpty(friendConnection))
            {
                try
                {
                    Clients.Client(friendConnection).GameRequest(gameRequest);
                }
                catch (Exception e)
                {
                    DeclineGameRequest(gameRequest);
                    return false;
                }

                Cache.AddPlayer(gameRequest.Sender);
                Cache.AddPlayer(gameRequest.Recipient);
                return true;
            }

            // something wrong appended
            return false;
        }

        public void AcceptGameRequest(GameRequestEntity gameRequest)
        {
            string senderConnection = ConnectionMapper.GetConnection(gameRequest.Sender.Id);
            if (string.IsNullOrEmpty(senderConnection))
            {
                GameService.CreateGame(gameRequest);

                Clients.Client(senderConnection).AcceptedGameRequest(gameRequest);
            }
        }

        public void DeclineGameRequest(GameRequestEntity gameRequest)
        {
            string senderConnection = ConnectionMapper.GetConnection(gameRequest.Sender.Id);
            if (!string.IsNullOrEmpty(senderConnection))
            {
                Cache.RemovePlayer(gameRequest.Sender);
                Cache.RemovePlayer(gameRequest.Recipient);
                Clients.Client(senderConnection).DeclinedGameRequest(gameRequest);
            }
        }

        public void CancelGameRequest(GameRequestEntity gameRequest)
        {
            string friendConnection = ConnectionMapper.GetConnection(gameRequest.Recipient.Id);

            if (!String.IsNullOrEmpty(friendConnection))
            {
                Clients.Client(friendConnection).GameRequestCanceled(gameRequest);
                Cache.RemovePlayer(gameRequest.Sender);
                Cache.RemovePlayer(gameRequest.Recipient);
            }
        }

        public async Task Logout(UserEntity user)
        {
            this.FriendService.NewUserDisconnected(user);
            Clients.AllExcept(this.Context.ConnectionId).NewFriendHasDisconnectedEvent(user.Id);

            base.Disconnect();
        }

        public bool FriendIsAvailable(int friendId)
        {
            return !Cache.PlayingPlayers.Exists(x => x.Id == friendId);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            // TODO ANY SPECIAL ACTION?

            return base.OnDisconnected(stopCalled);
        }
    }
}