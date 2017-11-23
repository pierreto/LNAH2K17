using InterfaceGraphique.CommunicationInterface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using System.Windows.Data;
using InterfaceGraphique.Entities;
using InterfaceGraphique.Services;

namespace InterfaceGraphique.Controls.WPF.Friends
{
    public class AddFriendListViewModel : ViewModelBase
    {
        #region Private Properties
        private FriendsHub friendsHub;
        private UserService userService;
        private TaskFactory ctxTaskFactory;
        private ObservableCollection<UserEntity> items;
        private AddUserViewModel addUserViewModel;

        #endregion

        #region Public Properties
        public ObservableCollection<UserEntity> Items
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

        public ICollectionView ItemsView
        {
            get { return CollectionViewSource.GetDefaultView(Items); }
        }
        #endregion

        #region Constructor
        public AddFriendListViewModel(FriendsHub friendsHub, UserService userService, AddUserViewModel addUserViewModel)
        {
            this.friendsHub = friendsHub;
            this.addUserViewModel = addUserViewModel;
            this.userService = userService;
            this.friendsHub.FriendRequestEvent += FriendRequestEvent;
            ctxTaskFactory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());
            Items = new ObservableCollection<UserEntity>();
            ItemsView.Filter = new Predicate<object>(o => Filter(o as UserEntity));
        }
        #endregion

        #region Private Methods
        private bool Filter(UserEntity flivm)
        {
            string username = addUserViewModel.FriendUsername;
            return username == null || username == "" || flivm.Username.IndexOf(username) != -1;
        }

        private void FriendRequestEvent(FriendRequestEntity request)
        {
            ctxTaskFactory.StartNew(() =>
            {
                //Ceci est envoye a la personne qui recoit un friend request.
                //On enleve le sender de la liste des gens qu'on peut ajouter
                //etant donne quil sera dans nos notifications
                System.Diagnostics.Debug.WriteLine(User.Instance.UserEntity.Username + " got a friend request from : " + request.Requestor.Username);
                Items.Remove(Items.Single(x => x.Username == request.Requestor.Username));
                //TODO: Add it to notification side
                var items = Program.unityContainer.Resolve<FriendRequestListViewModel>().Items;
                items.Add(new FriendListItemViewModel(new UserEntity { Id = request.Requestor.Id, Username = request.Requestor.Username, Profile = request.Requestor.Profile, IsSelected = false }, null) { RequestedFriend = true });
                Program.unityContainer.Resolve<FriendRequestListViewModel>().OnPropertyChanged("Items");
            }).Wait();
        }

        #endregion

        #region Overwritten Methods
        public async Task InitAddFriends()
        {
            List<UserEntity> users = await this.userService.GetAllUsers();
            //Don't add yourself or friends you already have
            //foreach (UserEntity user in users.Where(x => x.Username != User.Instance.UserEntity.Username))
            foreach (UserEntity user in users.Where(x => x.Username != User.Instance.UserEntity.Username && !Program.unityContainer.Resolve<FriendListViewModel>().FriendList.Any(y => x.Username == y.Username)))
            {
                Items.Add(user);
            }
            OnPropertyChanged(nameof(Items));
        }

        public override void InitializeViewModel()
        {
            //Rien
        }
        #endregion
    }
}
