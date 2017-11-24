using System.Threading.Tasks;
using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Entities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.Practices.Unity;
using InterfaceGraphique.Managers;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using InterfaceGraphique.Controls.WPF.Chat.Channel;

namespace InterfaceGraphique.Controls.WPF.Friends
{
    public class FriendListViewModel : MinimizableViewModelBase
    {
        private FriendsHub friendsHub;
        private bool hasNewFriendRequest;
        private bool hasNewFriend;
        private bool hasNewRequest;
        private TaskFactory ctxTaskFactory;
        private ObservableCollection<FriendListItemViewModel> friendList;

        public bool HasNewRequest
        {
            get
            {
                return hasNewRequest;
            }
            set
            {
                hasNewRequest = value;
                OnPropertyChanged(nameof(HasNewRequest));
            }
        }

        public bool HasNewFriend
        {
            get
            {
                return hasNewFriend;
            }
            set
            {
                hasNewFriend = value;
                OnPropertyChanged(nameof(HasNewFriend));
            }
        }

        public bool HasNewFriendRequest {
            get
            {
                return hasNewFriendRequest;
            }
                set
            {
                hasNewFriendRequest = value;
                OnPropertyChanged(nameof(HasNewFriendRequest));
            }
        }


        public UserEntity SelectedFriend { get; set; }

        public FriendListViewModel(FriendsHub friendsHub, GameRequestManager gameRequestManager)
        {
            this.friendsHub = friendsHub;
            ctxTaskFactory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());
            GameRequestManager = gameRequestManager;

           // ItemsView.SortDescriptions.Add(new SortDescription("IsConnected", ListSortDirection.Descending));

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

        public GameRequestManager GameRequestManager { get; }

        public async Task Init()
        {
            var friends = await friendsHub.GetAllFriends();
            FriendList = new ObservableCollection<FriendListItemViewModel>();
            foreach(var friend in friends)
            {
                FriendList.Add(new FriendListItemViewModel(new UserEntity { Id = friend.Id, Username = friend.Username, Profile = friend.Profile, IsSelected = false,IsConnected = friend.IsConnected}, GameRequestManager) { CurrentFriend = true });
            }
            //List<UserEntity> userEntities = await userService.GetAllUsers();
            this.friendsHub.NewFriendEvent += NewFriendEvent;
            this.friendsHub.RemovedFriendEvent += RemovedFriendEvent;
            this.friendsHub.NewFriendHasConnectedEvent += NewFriendHasConnectedEvent;
            this.friendsHub.NewFriendHasDisconnectedEvent += NewFriendHasDisconnectedEvent;
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
                HasNewRequest = false;
                Collapsed = System.Windows.Visibility.Visible;
                Program.FormManager.MaximizeFriendList();
            }
            Program.unityContainer.Resolve<AddUserViewModel>().Collapsed = Collapsed;
            Program.unityContainer.Resolve<AddUserViewModel>().TabIcon = TabIcon;
        }

        private void NewFriendEvent(UserEntity friend)
        {
            System.Diagnostics.Debug.WriteLine("Je viens d'ajouter " + friend.Username + " a mes amis.");
            ctxTaskFactory.StartNew(() =>
            {
                FriendList.Add(new FriendListItemViewModel(new UserEntity { Id = friend.Id, Username = friend.Username, Profile = friend.Profile, IsSelected = false, IsConnected = friend.IsConnected}, null) { CurrentFriend = true });
                var items = Program.unityContainer.Resolve<AddFriendListViewModel>().Items;
                HasNewFriend = true;
                items.Remove(items.Single(x => x.Id == friend.Id));
            }).Wait();
        }

        private void RemovedFriendEvent(UserEntity ex_friend)
        {
            ctxTaskFactory.StartNew(() =>
            {
                FriendList.Remove(FriendList.Single(x => x.Username == ex_friend.Username));
                Program.unityContainer.Resolve<AddFriendListViewModel>().Items.Add(new UserEntity { Id = ex_friend.Id, Username = ex_friend.Username, Profile = ex_friend.Profile, IsSelected = false,IsConnected = ex_friend.IsConnected});
            }).Wait();
        }

        public ICollectionView ItemsView
        {
            get { return CollectionViewSource.GetDefaultView(FriendList); }
        }
        private void NewFriendHasConnectedEvent(int userId)
        {
            for(int i = FriendList.Count - 1; i >= 0; i--)
            {
                if (FriendList[i].Id == userId)
                {
                    ctxTaskFactory.StartNew(() =>
                    {
                        FriendListItemViewModel vm = FriendList[i];
                        FriendList.RemoveAt(i);
                        vm.IsConnected = true;
                        FriendList.Add(vm);
                    }).Wait();
                    break;

                }
            }

        }
        private void NewFriendHasDisconnectedEvent(int userId)
        {
            for (int i = FriendList.Count - 1; i >= 0; i--)
            {
                if (FriendList[i].Id == userId)
                {
                    ctxTaskFactory.StartNew(() =>
                    {
                        Program.unityContainer.Resolve<ChatListViewModel>().Items.Remove(Program.unityContainer.Resolve<ChatListViewModel>().Items.Single(x => x.ChannelEntity.Name == FriendList[i].Username && x.ChannelEntity.IsPrivate));
                        Program.unityContainer.Resolve<ChatListViewModel>().OnPropertyChanged("Items");
                        FriendListItemViewModel vm = FriendList[i];
                        FriendList.RemoveAt(i);
                        vm.IsConnected = false;
                        FriendList.Add(vm);
                    }).Wait();
                    break;

                }
            }
        }

        public override void InitializeViewModel()
        {
            //Rien
        }
    }
}