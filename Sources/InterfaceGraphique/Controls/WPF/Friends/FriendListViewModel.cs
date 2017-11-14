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

namespace InterfaceGraphique.Controls.WPF.Friends
{
    public class FriendListViewModel : MinimizableViewModelBase
    {
        private FriendsHub friendsHub;
        private List<UserEntity> friendList;


        private ICommand removeFriendCommand;

        public UserEntity SelectedFriend { get; set; }

        public FriendListViewModel(FriendsHub friendsHub)
        {
            this.friendsHub = friendsHub;
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

  

        public override async void InitializeViewModel()
        {
            Minimize();

            FriendList = await this.friendsHub.GetAllFriends();
            //List<UserEntity> userEntities = await userService.GetAllUsers();
            this.friendsHub.NewFriendEvent += NewFriendEvent;
            this.friendsHub.RemovedFriendEvent += RemovedFriendEvent;


        }

        public override void Minimize()
        {
            if (Collapsed == System.Windows.Visibility.Visible)
            {
                TabIcon = "Users";
                Collapsed = System.Windows.Visibility.Collapsed;
                Program.FormManager.CurrentForm?.MinimizeFriendList();
            }
            else
            {
                TabIcon = "AngleDown";
                Collapsed = System.Windows.Visibility.Visible;
                Program.FormManager.CurrentForm?.MaximizeFriendList();
            }
            Program.unityContainer.Resolve<AddUserViewModel>().Collapsed = Collapsed;
            Program.unityContainer.Resolve<AddUserViewModel>().TabIcon = TabIcon;


        }


        public ICommand RemoveFriendCommand
        {
            get
            {
                return removeFriendCommand ??
                       (removeFriendCommand = new RelayCommandAsync(RemoveFriend));
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
        }

        private void NewFriendEvent(UserEntity friend)
        {
            updateLists();
        }



        private void RemovedFriendEvent(UserEntity ex_friend)
        {
            updateLists();
        }

    }
}