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
using Microsoft.Practices.Unity;
using InterfaceGraphique.Controls.WPF.Chat.Channel;

namespace InterfaceGraphique.Controls.WPF.Friends
{
    public class FriendListViewModel : MinimizableViewModelBase
    {
        private FriendsHub friendsHub;
        private ObservableCollection<FriendListItemViewModel> friendList;

        public UserEntity SelectedFriend { get; set; }

        public FriendListViewModel(FriendsHub friendsHub)
        {
            this.friendsHub = friendsHub;
        }

        public ObservableCollection<FriendListItemViewModel> FriendList
        {
            get => this.friendList;
            set
            {
                this.friendList = value;
                this.OnPropertyChanged();
            }
        }

        public override async void InitializeViewModel()
        {
            Minimize();
            var friends = await friendsHub.GetAllFriends();
            FriendList = new ObservableCollection<FriendListItemViewModel>();
            foreach(var friend in friends)
            {
                FriendList.Add(new FriendListItemViewModel(new UserEntity { Id = friend.Id, Username = friend.Username, IsSelected = false }));
            }
            //List<UserEntity> userEntities = await userService.GetAllUsers();
            this.friendsHub.NewFriendEvent += NewFriendEvent;
            this.friendsHub.RemovedFriendEvent += RemovedFriendEvent;
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
            Program.unityContainer.Resolve<AddUserViewModel>().Collapsed = Collapsed;
            Program.unityContainer.Resolve<AddUserViewModel>().TabIcon = TabIcon;
        }





        private void UpdateLists()
        {
            Task.Run(async () =>
            {
                var friends = await friendsHub.GetAllFriends();
                FriendList = new ObservableCollection<FriendListItemViewModel>();
                foreach (var friend in friends)
                {
                    FriendList.Add(new FriendListItemViewModel(new UserEntity { Username = friend.Username, IsSelected = false }));
                }
            }
            );
        }

        private void NewFriendEvent(UserEntity friend)
        {
            UpdateLists();
        }

        private void RemovedFriendEvent(UserEntity ex_friend)
        {
            UpdateLists();
        }

    }
}