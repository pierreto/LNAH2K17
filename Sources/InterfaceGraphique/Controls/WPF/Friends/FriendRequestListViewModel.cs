using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using System.Threading.Tasks;

namespace InterfaceGraphique.Controls.WPF.Friends
{
    public class FriendRequestListViewModel : ViewModelBase
    {
        #region Private Properties
        private FriendsHub friendsHub;
        private TaskFactory ctxTaskFactory;
        private ObservableCollection<FriendListItemViewModel> items;
        #endregion

        #region Public Properties
        public ObservableCollection<FriendListItemViewModel> Items
        {
            get
            {
                return items;
            }
            set
            {
                items = value;
                OnPropertyChanged(nameof(Items));
            }
        }
        #endregion

        #region Constructor
        public FriendRequestListViewModel(FriendsHub friendsHub)
        {
            this.friendsHub = friendsHub;
            Items = new ObservableCollection<FriendListItemViewModel>();
            ctxTaskFactory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());
        }
        #endregion

        #region Private Methods

        #endregion

        #region Overwritten Methods
        public async Task Init()
        {
            List<FriendRequestEntity> users = await friendsHub.GetAllPendingRequests();
            if (users.Count > 0)
            {
                Program.unityContainer.Resolve<FriendListViewModel>().HasNewRequest = true;
                Program.unityContainer.Resolve<FriendListViewModel>().HasNewFriendRequest = true;
            }
            foreach (FriendRequestEntity user in users)
            {
                Items.Add(new FriendListItemViewModel(new UserEntity { Id = user.Requestor.Id, Username = user.Requestor.Username, Profile = user.Requestor.Profile, IsSelected = false, IsConnected = user.Requestor.IsConnected }, null) { RequestedFriend = true });
            }
         }

        public override void InitializeViewModel()
        {
            //Rien
        }
        #endregion
    }
}
