using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.CommunicationInterface.RestInterface;
using InterfaceGraphique.Entities;
using InterfaceGraphique.Services;
using Microsoft.Practices.Unity;

namespace InterfaceGraphique.Controls.WPF.Friends
{
    public class AddUserViewModel : MinimizableViewModelBase
    {
        private FriendsHub friendsHub;
        private List<FriendRequestEntity> friendRequestList;
        private ICommand sendFriendRequestCommand;
        private string friendUsername;
        private ObservableCollection<string> usernames;
        private UserService userService;

        private ICommand acceptFriendRequestCommand;
        private ICommand refuseFriendRequestCommand;


        public AddUserViewModel(FriendsHub friendsHub, UserService userService)
        {
            this.friendsHub = friendsHub;
            this.userService = userService;
            this.Usernames = new ObservableCollection<string>();
        }

        public FriendRequestEntity SelectedFriendRequest { get; set; }


        public List<FriendRequestEntity> FriendRequestList
        {
            get => this.friendRequestList;
            set
            {
                this.friendRequestList = value;
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

        public string FriendUsername
        {
            get => friendUsername;
            set
            {
                friendUsername = value;
                this.OnPropertyChanged();
            }
        }
        public override async void InitializeViewModel()
        {
            this.Usernames.Clear();
            List<UserEntity> tempUsernames = await this.userService.GetAllUsers();
            foreach (UserEntity user in tempUsernames)
            {
                this.Usernames.Add(user.Name);
            }
            FriendRequestList = await this.friendsHub.GetAllPendingRequests();
            this.friendsHub.FriendRequestEvent += FriendRequestEvent;
            this.friendsHub.CanceledFriendRequestEvent += CanceledFriendRequestEvent;
        }

        public override void Minimize()
        {
            if (Collapsed == System.Windows.Visibility.Visible)
            {
                TabIcon = "Users";
                Collapsed = System.Windows.Visibility.Collapsed;
                Program.FormManager.MinimizeFriendList();
            }
            else
            {
                TabIcon = "AngleDown";
                Collapsed = System.Windows.Visibility.Visible;
                Program.FormManager.MaximizeFriendList();

            }
            Program.unityContainer.Resolve<FriendListViewModel>().Collapsed = Collapsed;
            Program.unityContainer.Resolve<FriendListViewModel>().TabIcon = TabIcon;

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


        public ICommand AcceptFriendRequestCommand
        {
            get
            {
                return acceptFriendRequestCommand ??
                       (acceptFriendRequestCommand = new RelayCommandAsync(AcceptFriendRequest));
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
        public ICommand RefuseFriendRequestCommand
        {
            get
            {
                return refuseFriendRequestCommand ??
                       (refuseFriendRequestCommand = new RelayCommandAsync(RefuseFriendRequest));
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

        private void updateLists()
        {
            Task.Run(async () =>
                FriendRequestList = await this.friendsHub.GetAllPendingRequests()
            );
        }
        private void FriendRequestEvent(FriendRequestEntity request)
        {
            updateLists();
        }

        private void CanceledFriendRequestEvent(FriendRequestEntity request)
        {
            updateLists();
        }
    }
}
