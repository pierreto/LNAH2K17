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
        #region Private Properties
        private string friendUsername;
        private FriendsHub friendsHub;
        private List<FriendRequestEntity> friendRequestList;
        private ObservableCollection<FriendListItemViewModel> friendsToAddList;
        private UserService userService;
        #endregion

        #region Public Properties
        public string FriendUsername
        {
            get => friendUsername;
            set
            {
                friendUsername = value;
                this.OnPropertyChanged();
                Program.unityContainer.Resolve<AddFriendListViewModel>().ItemsView.Refresh();
            }
        }
        #endregion

        #region Constructor
        public AddUserViewModel(FriendsHub friendsHub, UserService userService)
        {
            this.friendsHub = friendsHub;
            this.userService = userService;
            this.FriendsToAddList = new ObservableCollection<FriendListItemViewModel>();
        }
        #endregion

        //public List<FriendRequestEntity> FriendRequestList
        //{
        //    get => this.friendRequestList;
        //    set
        //    {
        //        this.friendRequestList = value;
        //        this.OnPropertyChanged();
        //    }
        //}

        public ObservableCollection<FriendListItemViewModel> FriendsToAddList
        {
            get => friendsToAddList;
            set
            {
                friendsToAddList = value;
                this.OnPropertyChanged();
            }
        }

        public override void InitializeViewModel()
        {
            //this.FriendsToAddList.Clear();
            //List<UserEntity> users = await this.userService.GetAllUsers();
            //foreach (UserEntity user in users)
            //{
            //    FriendsToAddList.Add(new FriendListItemViewModel(new UserEntity { Id = user.Id, Username = user.Username, Profile = user.Profile, IsSelected = false }, null) { AddingFriend = true });
            //}
            ////FriendRequestList = await this.friendsHub.GetAllPendingRequests();
            //this.friendsHub.FriendRequestEvent += FriendRequestEvent;
            ////this.friendsHub.CanceledFriendRequestEvent += CanceledFriendRequestEvent;
        }

        public override void Minimize()
        {
            if (Collapsed == System.Windows.Visibility.Visible)
            {
                TabIcon = "User";
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

        private void updateLists()
        {
            //Task.Run(async () =>
            //    FriendRequestList = await this.friendsHub.GetAllPendingRequests()
            //);
        }

        private void CanceledFriendRequestEvent(FriendRequestEntity request)
        {
            updateLists();
        }
    }
}
