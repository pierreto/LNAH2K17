using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AirHockeyServer.Repositories;
using System.Threading.Tasks;
using AirHockeyServer.Entities;

namespace AirHockeyServer.Services
{
    public class FriendService : IFriendService, IService
    {
        private FriendRequestRepository FriendRepository;

        public FriendService()
        {
            FriendRepository = new FriendRequestRepository();
        }

        public async Task<List<UserEntity>> GetAllFriends(UserEntity user)
        {
            return await FriendRepository.GetAllFriends(user.Id);
        }

        public async Task<FriendRequestEntity> SendFriendRequest(UserEntity user, UserEntity friend)
        {
            var request = new FriendRequestEntity{
                Requestor = user,
                Friend = friend,
                Status = RequestStatus.Pending
            };

            return await FriendRepository.SendFriendRequest(request);
        }

        public async Task<FriendRequestEntity> AcceptFriendRequest(FriendRequestEntity request)
        {
            return await FriendRepository.AcceptFriendRequest(request);
        }

        public async Task<FriendRequestEntity> RefuseFriendRequest(FriendRequestEntity request)
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
    }
}