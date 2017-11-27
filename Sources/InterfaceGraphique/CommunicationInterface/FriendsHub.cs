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
    public class FriendsHub : BaseHub, IBaseHub
    {
        private IHubProxy FriendsProxy { get; set; }
        private HubConnection Connection;
        private UserEntity user;

        public event Action<FriendRequestEntity> FriendRequestEvent;
        public event Action<UserEntity> NewFriendEvent;
        public event Action<UserEntity> NewAddableFriendEvent;
        public event Action<UserEntity> RemovedFriendEvent;
        public event Action<FriendRequestEntity> CanceledFriendRequestEvent;

        public event Action<GameRequestEntity> GameRequestEvent;
        public event Action<GameRequestEntity> DeclinedGameRequestEvent;
        public event Action<GameRequestEntity> AcceptedGameRequestEvent;
        public event Action<GameRequestEntity> GameRequestCanceledEvent;
        public event Action<int> NewFriendHasConnectedEvent;
        public event Action<int> NewFriendHasDisconnectedEvent;

        public void InitializeHub(HubConnection connection)
        {
            System.Diagnostics.Debug.WriteLine("InitializeHub");
            Connection = connection;
            FriendsProxy = Connection.CreateHubProxy("FriendsHub");
            InitializeEvents();
        }

        public async Task InitializeFriendsHub()
        {
            var me = User.Instance.UserEntity;
            this.user = new UserEntity()
            {
                Username = me.Username,
                AlreadyPlayedGame = me.AlreadyPlayedGame,
                AlreadyUsedFatEditor = me.AlreadyUsedFatEditor,
                AlreadyUsedLightEditor = me.AlreadyUsedLightEditor,
                Created = me.Created,
                Email = me.Email,
                Id = me.Id,
                IsConnected = me.IsConnected,
                IsSelected = me.IsSelected,
                Name = me.Name,
                //Profile = me.Profile
            };
            try
            {
                await FriendsProxy.Invoke("JoinHub", user);
            }
            catch (Exception e)
            {
                HandleError("FriendsHub -> InitializeFriendsHub");
            }
        }

        private void InitializeEvents()
        {
            FriendsProxy.On<FriendRequestEntity>("FriendRequestEvent", friendRequest =>
            {
                FriendRequestEvent?.Invoke(friendRequest);
            });

            FriendsProxy.On<UserEntity>("NewFriendEvent", friend =>
            {
                NewFriendEvent?.Invoke(friend);
            });

            FriendsProxy.On<UserEntity>("NewAddableFriendEvent", friend =>
            {
                NewAddableFriendEvent?.Invoke(friend);
            });

            FriendsProxy.On<UserEntity>("RemovedFriendEvent", ex_friend =>
            {
                RemovedFriendEvent?.Invoke(ex_friend);
            });

            FriendsProxy.On<FriendRequestEntity>("CanceledFriendRequestEvent", request =>
            {
                CanceledFriendRequestEvent?.Invoke(request);
            });

            FriendsProxy.On<GameRequestEntity>("GameRequest", request =>
            {
                GameRequestEvent?.Invoke(request);
            });

            FriendsProxy.On<GameRequestEntity>("DeclinedGameRequest", request =>
            {
                DeclinedGameRequestEvent?.Invoke(request);
            });

            FriendsProxy.On<GameRequestEntity>("AcceptedGameRequest", request =>
            {
                AcceptedGameRequestEvent?.Invoke(request);
            });

            FriendsProxy.On<int>("NewFriendHasConnectedEvent", request =>
            {
                NewFriendHasConnectedEvent?.Invoke(request);
            });

            FriendsProxy.On<int>("NewFriendHasDisconnectedEvent", request =>
            {
                NewFriendHasDisconnectedEvent?.Invoke(request);
            });

            FriendsProxy.On<GameRequestEntity>("GameRequestCanceled", request => { GameRequestCanceledEvent?.Invoke(request); });
        }

        public async Task<bool> FriendIsAvailable(int recipientId)
        {
            try
            {
                return await FriendsProxy?.Invoke<bool>("FriendIsAvailable", recipientId);
            }
            catch (Exception e)
            {
                HandleError("FriendsHub -> FriendIsAvailable");
            }

            return false;
        }

        public async Task<List<UserEntity>> GetAllFriends()
        {
            try
            {
                return await FriendsProxy?.Invoke<List<UserEntity>>("GetAllFriends", this.user);
            }
            catch (Exception e)
            {
                HandleError("FriendsHub -> GetAllFriends");
            }
            return null;
        }

        public async Task<List<FriendRequestEntity>> GetAllPendingRequests()
        {
            try
            {

                return await FriendsProxy?.Invoke<List<FriendRequestEntity>>("GetAllPendingRequests", this.user);
            }
            catch (Exception e)
            {
                HandleError("FriendsHub -> GetAllPendingRequests");
            }
            return null;
        }

        public async Task<FriendRequestEntity> SendFriendRequest(UserEntity friend)
        {
            try
            {

                return await FriendsProxy?.Invoke<FriendRequestEntity>("SendFriendRequest", this.user, friend);
            }
            catch (Exception e)
            {
                HandleError("FriendsHub -> SendFriendRequest");
            }
            return null;
        }

        public async Task SignalSignup()
        {
            try
            {

                await FriendsProxy?.Invoke("SignalSignup", this.user);
            }
            catch (Exception e)
            {
                HandleError("FriendsHub -> SignalSignup");
            }
        }

        public async Task<bool> AcceptFriendRequest(FriendRequestEntity request)
        {
            FriendRequestEntity res = new FriendRequestEntity();
            try
            {
                res = await FriendsProxy?.Invoke<FriendRequestEntity>("AcceptFriendRequest", request);
            }
            catch (Exception e)
            {
                HandleError("FriendsHub -> AcceptFriendRequest");
            }
            return (res != null) ? true : false;
        }

        public async Task<bool> RefuseFriendRequest(FriendRequestEntity request)
        {
            FriendRequestEntity res = new FriendRequestEntity();
            try
            {
                res = await FriendsProxy?.Invoke<FriendRequestEntity>("RefuseFriendRequest", request);
            }
            catch (Exception e)
            {
                HandleError("FriendsHub -> RefuseFriendRequest");
            }
            return (res != null) ? true : false;
        }
        public async Task<bool> RemoveFriend(UserEntity ex_friend)
        {
            try
            {
                return await FriendsProxy?.Invoke<bool>("RemoveFriend", this.user, ex_friend);
            }
            catch (Exception e)
            {
                HandleError("FriendsHub - > RemoveFriend");
            }
            return false;
        }

        public async Task AcceptGameRequest(GameRequestEntity gameRequest)
        {
            try
            {
                await FriendsProxy?.Invoke("AcceptGameRequest", gameRequest);
            }
            catch (Exception e)
            {
                HandleError("FriendsHub -> AcceptGameRequest");
            }
        }

        public async Task DeclineGameRequest(GameRequestEntity gameRequest)
        {
            try
            {
                await FriendsProxy?.Invoke("DeclineGameRequest", gameRequest);
            }
            catch (Exception e)
            {
                HandleError("FriendsHub -> DeclineGameRequest");
            }
        }

        public async Task<bool> SendGameRequest(GameRequestEntity gameRequest)
        {
            try
            {
                return await FriendsProxy?.Invoke<bool>("SendGameRequest", gameRequest);
            }
            catch (Exception e)
            {
                HandleError("FriendsHub -> SendGameRequest");
            }

            return false;
        }

        public async Task CancelGameRequest(GameRequestEntity gameRequest)
        {
            try
            {
                await FriendsProxy?.Invoke("CancelGameRequest", gameRequest);
            }
            catch (Exception e)
            {
                HandleError("FriendsHub -> CancelGameRequest");
            }
        }

        public async Task Logout()
        {
            try
            {
                await FriendsProxy?.Invoke("Logout", User.Instance.UserEntity);
            }
            catch (Exception e)
            {
                HandleError("FriendsHub -> Logout");
            }
        }

        public async Task LeaveRoom()
        {
            // do nothing
            return;
        }
    }
}