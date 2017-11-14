using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Entities;

namespace InterfaceGraphique.Controls.WPF.Friends
{
    public class AddUserViewModel : ViewModelBase
    {
        private FriendsHub friendsHub;
        private List<FriendRequestEntity> friendRequestList;

        private ICommand acceptFriendRequestCommand;
        private ICommand refuseFriendRequestCommand;
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

        public AddUserViewModel(FriendsHub friendsHub)
        {
            this.friendsHub = friendsHub;
        }

        public override async void InitializeViewModel()
        {
            FriendRequestList = await this.friendsHub.GetAllPendingRequests();
            this.friendsHub.FriendRequestEvent += FriendRequestEvent;
            this.friendsHub.CanceledFriendRequestEvent += CanceledFriendRequestEvent;
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
