using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Web;
using AirHockeyServer.DatabaseCore;
using AirHockeyServer.Entities;
using AirHockeyServer.Pocos;
using System.Threading.Tasks;
using AirHockeyServer.Mapping;

namespace AirHockeyServer.Repositories
{
    public class FriendRequestRepository : Repository<MapRepository>
    {
        private Table<FriendPoco> FriendRequests;

        public FriendRequestRepository()
        {
            FriendRequests = DataProvider.DC.GetTable<FriendPoco>();
        }

        public async Task<List<FriendRequestEntity>> GetAllFriends(int user_id)
        {
            try
            {
                IQueryable<FriendPoco> queryable =
                    from friend_request in this.FriendRequests
                    where (friend_request.Requestor == user_id || friend_request.Friend == user_id) && friend_request.Status == 1
                    select friend_request;

                var results = await Task<List<FriendPoco>>.Run(
                    () => queryable.ToList<FriendPoco>());

                return MapperManager.Map<List<FriendPoco>, List<FriendRequestEntity>>(results);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[FriendRequestRepository.GetAllFriendRequests] " + e.ToString());
                return null;
            }
        }

        public async Task<List<FriendRequestEntity>> GetAllPendingRequests(int user_id)
        {
            try
            {
                IQueryable<FriendPoco> queryable =
                    from friend_request in this.FriendRequests
                    where (friend_request.Friend == user_id) && friend_request.Status == 0
                    select friend_request;

                var results = await Task<List<FriendPoco>>.Run(
                    () => queryable.ToList<FriendPoco>());

                return MapperManager.Map<List<FriendPoco>, List<FriendRequestEntity>>(results);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[FriendRequestRepository.GetAllPendingRequests] " + e.ToString());
                return null;
            }
        }

        public async Task SendFriendRequest(FriendRequestEntity request)
        {
            try
            {
                FriendPoco newFriendRequest = MapperManager.Map<FriendRequestEntity, FriendPoco>(request);
                this.FriendRequests.InsertOnSubmit(newFriendRequest);
                await Task.Run(() => this.DataProvider.DC.SubmitChanges());
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[FriendRequestRepository.SendFriendRequest] " + e.ToString());
            }
        }

        private async Task ModifyFriendRequest(FriendRequestEntity request, int status)
        {
            try
            {
                var query =
                    from friend_request in this.FriendRequests
                    where friend_request.Requestor == request.Requestor && friend_request.Friend == request.Friend
                    select friend_request;

                var friendRequest = query.ToArray().First();
                friendRequest.Status = status;
                await Task.Run(() => this.DataProvider.DC.SubmitChanges());
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[FriendRequestRepository.ModifyFriendRequest] " + e.ToString());
            }
        }

        public async Task AcceptFriendRequest(FriendRequestEntity request)
        {
            await ModifyFriendRequest(request, 1); 
        }

        public async Task RefuseFriendRequest(FriendRequestEntity request)
        {
            await ModifyFriendRequest(request, -1);
        }

        private async Task RemoveRelation(int user_id1, int user_id2)
        {
            try
            {
                var query =
                    from friend_request in this.FriendRequests
                    where (friend_request.Requestor == user_id1 && friend_request.Friend == user_id2) ||
                          (friend_request.Requestor == user_id2 && friend_request.Friend == user_id1)
                    select friend_request;

                var relation = query.ToArray().First();
                this.FriendRequests.DeleteOnSubmit(relation);
                await Task.Run(() => this.DataProvider.DC.SubmitChanges());
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[FriendRequestRepository.RemoveRelation] " + e.ToString());
            }
        }

        public async Task CancelFriendRequest(FriendRequestEntity request)
        {
            await RemoveRelation(request.Requestor, request.Friend);
        }

        public async Task RemoveFriend(int user, int friend)
        {
            await RemoveRelation(user, friend);
        }
    }
}