using System.Threading.Tasks;
using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Entities;
using System.Collections.ObjectModel;
using Microsoft.Practices.Unity;
using InterfaceGraphique.Managers;
using System.Linq;

namespace InterfaceGraphique.Controls.WPF.Friends
{
    public class FriendListViewModel : MinimizableViewModelBase
    {
        private FriendsHub friendsHub;
        private TaskFactory ctxTaskFactory;
        private ObservableCollection<FriendListItemViewModel> friendList;

        public UserEntity SelectedFriend { get; set; }

        public FriendListViewModel(FriendsHub friendsHub, GameRequestManager gameRequestManager)
        {
            this.friendsHub = friendsHub;
            ctxTaskFactory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());
            GameRequestManager = gameRequestManager;
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

        public override async void InitializeViewModel()
        {
            Minimize();
            var friends = await friendsHub.GetAllFriends();
            FriendList = new ObservableCollection<FriendListItemViewModel>();
            foreach(var friend in friends)
            {
                FriendList.Add(new FriendListItemViewModel(new UserEntity { Id = friend.Id, Username = friend.Username, Profile = friend.Profile, IsSelected = false }, GameRequestManager) { CurrentFriend = true });
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

        private void NewFriendEvent(UserEntity friend)
        {
            ctxTaskFactory.StartNew(() =>
            {
                FriendList.Add(new FriendListItemViewModel(new UserEntity { Id = friend.Id, Username = friend.Username, Profile = friend.Profile, IsSelected = false }, null) { CurrentFriend = true });
                var items = Program.unityContainer.Resolve<AddFriendListViewModel>().Items;
                items.Remove(items.Single(x => x.Id == friend.Id));
            }).Wait();
        }

        private void RemovedFriendEvent(UserEntity ex_friend)
        {
            ctxTaskFactory.StartNew(() =>
            {
                FriendList.Remove(FriendList.Single(x => x.Username == ex_friend.Username));
                Program.unityContainer.Resolve<AddFriendListViewModel>().Items.Add(new FriendListItemViewModel(new UserEntity { Id = ex_friend.Id, Username = ex_friend.Username, Profile = ex_friend.Profile, IsSelected = false }, null) { AddingFriend = true });
            }).Wait();
        }
    }
}