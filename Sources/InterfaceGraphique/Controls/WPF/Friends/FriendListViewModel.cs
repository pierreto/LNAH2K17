using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Entities;
using InterfaceGraphique.CommunicationInterface.RestInterface;
using System.Net.Http;
using InterfaceGraphique.Services;
using System.Collections.ObjectModel;

namespace InterfaceGraphique.Controls.WPF.Friends
{
    public class FriendListViewModel : ViewModelBase
    {
        private FriendsHub friendsHub;
        private UserService userService;
        private List<UserEntity> friendList;
        private List<FriendRequestEntity> friendRequestList;
        private string friendUsername;
        private ObservableCollection<string> usernames;

        private ICommand sendFriendRequestCommand;
        private ICommand acceptFriendRequestCommand;
        private ICommand refuseFriendRequestCommand;
        private ICommand removeFriendCommand;

        public UserEntity SelectedFriend { get; set; }
        public FriendRequestEntity SelectedFriendRequest { get; set; }

        public FriendListViewModel(FriendsHub friendsHub, UserService userService)
        {
            this.friendsHub = friendsHub;
            this.userService = userService;
        }

        public List<UserEntity> FriendList
        {
            get => this.friendList;
            set
            {
                this.friendList = value;
                this.OnPropertyChanged();
            }
        }

        public List<FriendRequestEntity> FriendRequestList
        {
            get => this.friendRequestList;
            set
            {
                this.friendRequestList = value;
                this.OnPropertyChanged();
            }
        }

        public override async void InitializeViewModel()
        {
            await this.friendsHub.InitializeFriendsHub(User.Instance.UserEntity);
            FriendList = await this.friendsHub.GetAllFriends();
            FriendRequestList = await this.friendsHub.GetAllPendingRequests();

            List<UserEntity> userEntities = await userService.GetAllUsers();
            //Usernames = new ObservableCollection<string>(userEntities.Select(user => user.Username));

            this.friendsHub.NewFriendEvent += NewFriendEvent;
            this.friendsHub.FriendRequestEvent += FriendRequestEvent;
            this.friendsHub.RemovedFriendEvent += RemovedFriendEvent;
            this.friendsHub.CanceledFriendRequestEvent += CanceledFriendRequestEvent;
        }

        public string FriendUsername
        {
            get => friendUsername;
            set
            {
                friendUsername = value;
                this.OnPropertyChanged();
            }
        }

        public ObservableCollection<string> Usernames
        {
            get => usernames;
            set
            {
                usernames = value;
                this.OnPropertyChanged();
            }
        }

        public ICommand SendFriendRequestCommand
        {
            get
            {
                return sendFriendRequestCommand ??
                       (sendFriendRequestCommand = new RelayCommandAsync(SendFriendRequest));
            }
        }

        public ICommand AcceptFriendRequestCommand
        {
            get
            {
                return acceptFriendRequestCommand ??
                       (acceptFriendRequestCommand = new RelayCommandAsync(AcceptFriendRequest));
            }
        }

        public ICommand RefuseFriendRequestCommand
        {
            get
            {
                return refuseFriendRequestCommand ??
                       (refuseFriendRequestCommand = new RelayCommandAsync(RefuseFriendRequest));
            }
        }

        public ICommand RemoveFriendCommand
        {
            get
            {
                return removeFriendCommand ??
                       (removeFriendCommand = new RelayCommandAsync(RemoveFriend));
            }
        }

        private async Task SendFriendRequest()
        {
            if (FriendUsername != null)
            {
                HttpResponseMessage response = await Program.client.GetAsync("api/user/u/" + FriendUsername);
                UserEntity friend = await HttpResponseParser.ParseResponse<UserEntity>(response);
                await this.friendsHub.SendFriendRequest(friend);
            }
        }

        private async Task AcceptFriendRequest()
        {
            if (SelectedFriendRequest != null)
            {
                await this.friendsHub.AcceptFriendRequest(SelectedFriendRequest);
            }
        }

       private async Task RefuseFriendRequest()
        {
            if (SelectedFriendRequest != null)
            {
                if (await this.friendsHub.RefuseFriendRequest(SelectedFriendRequest))
                    updateLists();
            }
        }

        private async Task RemoveFriend()
        {
            if (SelectedFriend != null)
            {
                await this.friendsHub.RemoveFriend(SelectedFriend);
            }
        }

        private void updateLists()
        {
            Task.Run(async () =>
                FriendList = await this.friendsHub.GetAllFriends()
            );
            Task.Run(async () =>
                FriendRequestList = await this.friendsHub.GetAllPendingRequests()
            );
        }

        private void NewFriendEvent(UserEntity friend)
        {
            updateLists();
        }

        private void FriendRequestEvent(FriendRequestEntity request)
        {
            updateLists();
        }

        private void RemovedFriendEvent(UserEntity ex_friend)
        {
            updateLists();
        }

        private void CanceledFriendRequestEvent(FriendRequestEntity request)
        {
            updateLists();
        }
    }
}