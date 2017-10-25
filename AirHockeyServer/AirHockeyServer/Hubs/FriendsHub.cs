using System;
using System.Collections.Generic;
using Microsoft.AspNet.SignalR;
using System.Linq;
using System.Web;
using AirHockeyServer.Services;
using AirHockeyServer.Entities;
using System.Threading.Tasks;
using AirHockeyServer.Events;

namespace AirHockeyServer.Hubs
{
    public class FriendsHub : Hub 
    {
        protected IFriendService FriendService { get; }

        public FriendsHub(IFriendService friendService)
        {
            FriendService = friendService;
        }

        public void JoinHub()
        //public void JoinHub(UserEntity user)
        {
            System.Diagnostics.Debug.WriteLine("JOIN FUCKING HUB");
            //ConnectionMapper.AddConnection(user.Id, Context.ConnectionId);
        }

        public async Task<List<UserEntity>> GetAllFriends(UserEntity user)
        {
            return await FriendService.GetAllFriends(user);
        }

        public async Task<FriendRequestEntity> SendFriendRequest(UserEntity user, UserEntity friend)
        {
            var friendRequest = await FriendService.SendFriendRequest(user, friend);

            // On notifie l'utilisateur dont on veut être l'ami de la demande:
            if (friendRequest != null)
            {
                Clients.Client(ConnectionMapper.GetConnection(friend.Id)).FriendRequestEvent(friendRequest);
            }

            return friendRequest;
        }
        
        public async Task<FriendRequestEntity> AcceptFriendRequest(FriendRequestEntity request)
        {
            var relation = await FriendService.AcceptFriendRequest(request);

            // Si la demande d'ami a été acceptée avec succès, on notifie les deux
            // nouveaux amis (requestor et friend) :
            if (relation != null)
            {
                Clients.Client(ConnectionMapper.GetConnection(relation.Requestor.Id)).NewFriendEvent(relation.Friend);
                Clients.Client(ConnectionMapper.GetConnection(relation.Friend.Id)).NewFriendEvent(relation.Requestor);
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
            if (canceled_request)
            {
                Clients.Client(ConnectionMapper.GetConnection(request.Friend.Id)).CanceledFriendRequestEvent(request);
            }

            return canceled_request;
        }

        public async Task<bool> RemoveFriend(UserEntity user, UserEntity ex_friend)
        {
            var removed_friend = await FriendService.RemoveFriend(user, ex_friend);

            // Si la relation a été supprimée avec succès, on notifie chaque utilisateur de la
            // perte de son ami :
            if (removed_friend)
            {
                Clients.Client(ConnectionMapper.GetConnection(user.Id)).RemovedFriendEvent(ex_friend);
                Clients.Client(ConnectionMapper.GetConnection(ex_friend.Id)).RemovedFriendEvent(user);
            }

            return removed_friend;
        }
    }
}