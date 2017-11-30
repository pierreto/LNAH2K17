using AirHockeyServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirHockeyServer.Repositories.Interfaces
{
    public interface IFriendRequestRepository
    {
        Task<List<UserEntity>> GetAllFriends(int user_id);
        Task<List<FriendRequestEntity>> GetAllPendingRequests(int user_id);
        Task<FriendRequestEntity> SendFriendRequest(FriendRequestEntity request);
        Task<FriendRequestEntity> AcceptFriendRequest(FriendRequestEntity request);
        Task<bool> RefuseFriendRequest(FriendRequestEntity request);
        Task<bool> CancelFriendRequest(FriendRequestEntity request);
        Task<bool> RemoveFriend(int user, int friend);
    }
}