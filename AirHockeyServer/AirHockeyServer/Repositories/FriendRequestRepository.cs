﻿using System;
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
    public class FriendRequestRepository : Repository<FriendRequestRepository>
    {
        private Table<UserPoco> Users;
        private Table<FriendPoco> FriendRequests;

        public FriendRequestRepository()
        {
            try
            {
                Users = DataProvider.DC.GetTable<UserPoco>();
                FriendRequests = DataProvider.DC.GetTable<FriendPoco>();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[FriendRequestRepository()] " + e.ToString());
            }
        }

        public async Task<List<UserEntity>> GetAllFriends(int user_id)
        {
            try
            {
                IQueryable<FriendPoco> queryable =
                    from friend_request in this.FriendRequests
                    where (friend_request.RequestorID == user_id || friend_request.FriendID == user_id) && friend_request.Status == 1
                    select friend_request;

                var results = await Task<List<FriendPoco>>.Run(
                    () => queryable.ToList<FriendPoco>());

                // On extrait la liste d'amis à partir de la liste des demandes d'amis acceptées où apparaît l'id
                // de l'utilisateur :
                // - si l'utilisateur est l'initiateur de la demande, son ami est relation.friend
                // - si l'utilisateur a accepté la demande, son ami est l'initiateur relation.requestor:
                List<UserPoco> friends = results.Select(
                    relation => (relation.RequestorID == user_id) ? relation.Friend : relation.Requestor).ToList();

                return MapperManager.Map<List<UserPoco>, List<UserEntity>>(friends);
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
                    where (friend_request.FriendID == user_id) && friend_request.Status == 0
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

        public async Task<FriendRequestEntity> SendFriendRequest(FriendRequestEntity request)
        {
            try
            {
                UserPoco requestor =
                    (from user in this.Users where user.Id == request.Requestor.Id select user).ToArray<UserPoco>().First();
                UserPoco friend =
                    (from user in this.Users where user.Id == request.Friend.Id select user).ToArray<UserPoco>().First();

                FriendPoco newFriendRequest = MapperManager.Map<FriendRequestEntity, FriendPoco>(request);
                newFriendRequest.Requestor = requestor;
                newFriendRequest.Friend = friend;

                this.FriendRequests.InsertOnSubmit(newFriendRequest);
                await Task.Run(() => this.DataProvider.DC.SubmitChanges());

                return request;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[FriendRequestRepository.SendFriendRequest] " + e.ToString());
                return null;
            }
        }

        private async Task<FriendRequestEntity> ModifyFriendRequest(FriendRequestEntity request, int status)
        {
            try
            {
                var query =
                    from friend_request in this.FriendRequests
                    where friend_request.RequestorID == request.Requestor.Id && friend_request.FriendID == request.Friend.Id
                    select friend_request;

                var friendRequest = query.ToArray().First();
                friendRequest.Status = status;
                await Task.Run(() => this.DataProvider.DC.SubmitChanges());
                return MapperManager.Map<FriendPoco, FriendRequestEntity>(friendRequest);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[FriendRequestRepository.ModifyFriendRequest] " + e.ToString());
                return null;
            }
        }

        public async Task<FriendRequestEntity> AcceptFriendRequest(FriendRequestEntity request)
        {
            return await ModifyFriendRequest(request, 1); 
        }

        public async Task<FriendRequestEntity> RefuseFriendRequest(FriendRequestEntity request)
        {
            return await ModifyFriendRequest(request, -1);
        }

        private async Task<bool> RemoveRelation(int user_id1, int user_id2)
        {
            try
            {
                var query =
                    from friend_request in this.FriendRequests
                    where (friend_request.RequestorID == user_id1 && friend_request.FriendID == user_id2) ||
                          (friend_request.RequestorID == user_id2 && friend_request.FriendID == user_id1)
                    select friend_request;

                var relation = query.ToArray().First();
                this.FriendRequests.DeleteOnSubmit(relation);
                await Task.Run(() => this.DataProvider.DC.SubmitChanges());
                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[FriendRequestRepository.RemoveRelation] " + e.ToString());
                return false;
            }
        }

        public async Task<bool> CancelFriendRequest(FriendRequestEntity request)
        {
            return await RemoveRelation(request.Requestor.Id, request.Friend.Id);
        }

        public async Task<bool> RemoveFriend(int user, int friend)
        {
            return await RemoveRelation(user, friend);
        }
    }
}