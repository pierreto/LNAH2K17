using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AirHockeyServer.Repositories;
using System.Threading.Tasks;
using AirHockeyServer.Entities;
using AirHockeyServer.Services.Interfaces;
using AirHockeyServer.Repositories.Interfaces;

namespace AirHockeyServer.Services
{
    public class FriendService : IFriendService
    {
        public List<UserEntity> UsersIdConnected { get; set; }

        private IFriendRequestRepository FriendRepository { get; set; }
 
        public FriendService(IFriendRequestRepository friendRequestRepository)
        {
            FriendRepository = friendRequestRepository;
            this.UsersIdConnected = new List<UserEntity>();
        }

        public async Task<List<UserEntity>> GetAllFriends(UserEntity user)
        {
            return await FriendRepository.GetAllFriends(user.Id);
        }

        public async Task<List<FriendRequestEntity>> GetAllPendingRequests(UserEntity user)
        {
            return await FriendRepository.GetAllPendingRequests(user.Id);
        }

        public async Task<FriendRequestEntity> SendFriendRequest(UserEntity user, UserEntity friend)
        {
            // On empeche l'utilisateur de s'envoyer une demande a lui-meme:
            if (user.Id != friend.Id)
            {
                var request = new FriendRequestEntity
                {
                    Requestor = user,
                    Friend = friend,
                    Status = RequestStatus.Pending
                };

                return await FriendRepository.SendFriendRequest(request);
            }

            return null;
        }

        public async Task<FriendRequestEntity> AcceptFriendRequest(FriendRequestEntity request)
        {
            return await FriendRepository.AcceptFriendRequest(request);
        }

        public async Task<bool> RefuseFriendRequest(FriendRequestEntity request)
        {
            return await FriendRepository.RefuseFriendRequest(request);
        }

        public async Task<bool> CancelFriendRequest(FriendRequestEntity request)
        {
            return await FriendRepository.CancelFriendRequest(request);
        }

        public async Task<bool> RemoveFriend(UserEntity user, UserEntity ex_friend)
        {
            return await FriendRepository.RemoveFriend(user.Id, ex_friend.Id);
        }

        public void NewUserConnected(UserEntity userid)
        {
            UsersIdConnected.Add(userid);
        }
        public void NewUserDisconnected(UserEntity userid)
        {
            UsersIdConnected.Remove(userid);
        }
    }
}