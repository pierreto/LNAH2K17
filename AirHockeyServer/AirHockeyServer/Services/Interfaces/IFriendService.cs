using AirHockeyServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AirHockeyServer.Services
{
    public interface IFriendService
    {
        Task<List<UserEntity>> GetAllFriends(UserEntity user);

        Task<List<FriendRequestEntity>> GetAllPendingRequests(UserEntity user);

        Task<FriendRequestEntity> SendFriendRequest(UserEntity user, UserEntity friend);

        Task<FriendRequestEntity> AcceptFriendRequest(FriendRequestEntity request);

        Task<FriendRequestEntity> RefuseFriendRequest(FriendRequestEntity request);

        Task<bool> CancelFriendRequest(FriendRequestEntity request);

        Task<bool> RemoveFriend(UserEntity user, UserEntity ex_friend);
    }
}