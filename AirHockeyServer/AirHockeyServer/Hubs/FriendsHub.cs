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
    public class FriendsHub : Hub 
    {
        protected IFriendService FriendService { get; }
        public ConnectionMapper ConnectionMapper { get; }
        public GameService GameService { get; }

        public FriendsHub(IFriendService friendService, ConnectionMapper connectionMapper, GameService gameService)       
        {
            FriendService = friendService;
            ConnectionMapper = connectionMapper;
            GameService = gameService;
        }

        public void JoinHub(UserEntity user)
        {
            ConnectionMapper.AddConnection(user.Id, Context.ConnectionId);
        }

        public async Task<List<UserEntity>> GetAllFriends(UserEntity user)
        {
            return await FriendService.GetAllFriends(user);
        }

        public async Task<List<FriendRequestEntity>> GetAllPendingRequests(UserEntity user)
        {
            return await FriendService.GetAllPendingRequests(user);
        }

        public async Task<FriendRequestEntity> SendFriendRequest(UserEntity user, UserEntity friend)
        {
            var friendRequest = await FriendService.SendFriendRequest(user, friend);

            // On notifie l'utilisateur dont on veut être l'ami de la demande:
            string friendConnection = ConnectionMapper.GetConnection(friend.Id);
            if (friendRequest != null && friendConnection.Length > 0) 
            {
                Clients.Client(friendConnection).FriendRequestEvent(friendRequest);
            }

            return friendRequest;
        }
        
        public async Task<FriendRequestEntity> AcceptFriendRequest(FriendRequestEntity request)
        {
            var relation = await FriendService.AcceptFriendRequest(request);

            // Si la demande d'ami a été acceptée avec succès, on notifie les deux
            // nouveaux amis (requestor et friend) :
            string friendConnection = ConnectionMapper.GetConnection(relation.Friend.Id);
            if (relation != null)
            {
                Clients.Client(ConnectionMapper.GetConnection(relation.Requestor.Id)).NewFriendEvent(relation.Friend);

                if (friendConnection.Length > 0)
                    Clients.Client(friendConnection).NewFriendEvent(relation.Requestor);
            }

            return relation;
        }

        public async Task<FriendRequestEntity> RefuseFriendRequest(FriendRequestEntity request)
        {
            return await FriendService.RefuseFriendRequest(request);
        }
        
        public async Task<bool> CancelFriendRequest(FriendRequestEntity request)

        {
            var canceled_request = await FriendService.CancelFriendRequest(request);

            // Si la demande a été annulée avec succès, on notifie le client à qui on a envoyé
            // la demande :
            string friendConnection = ConnectionMapper.GetConnection(request.Friend.Id);
            if (canceled_request && friendConnection.Length > 0)
            {
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
                    Clients.Client(ex_friendConnection).RemovedFriendEvent(user);
            }

            return removed_friend;
        }

        public bool SendGameRequest(GameRequestEntity gameRequest)
        {
            if(Cache.PlayingPlayers.Exists(x => x.Id == gameRequest.Recipient.Id))
            {
                // player is playing
                return false;
            }

            string friendConnection = ConnectionMapper.GetConnection(gameRequest.Recipient.Id);

            if (friendConnection != null)
            {
                Clients.Client(friendConnection).GameRequest(gameRequest);
                return true;
            }

            // something wrong appended
            return false;
        }

        public void AcceptGameRequest(GameRequestEntity gameRequest)
        {
            string friendConnection = ConnectionMapper.GetConnection(gameRequest.Sender.Id);
            if (friendConnection != null)
            {
                Cache.AddPlayer(gameRequest.Sender);
                Cache.AddPlayer(gameRequest.Recipient);

                GameService.CreateGame(gameRequest);

                Clients.Client(friendConnection).AcceptGameRequest(gameRequest);
            }
        }

        public void DeclineGameRequest(GameRequestEntity gameRequest)
        {
            string friendConnection = ConnectionMapper.GetConnection(gameRequest.Sender.Id);
            if (friendConnection != null)
            {
                Clients.Client(friendConnection).DeclineGameRequest(gameRequest);
            }
        }
    }
}