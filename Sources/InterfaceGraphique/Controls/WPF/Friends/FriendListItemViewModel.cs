using InterfaceGraphique.Entities;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Unity;
using InterfaceGraphique.Controls.WPF.Friends;
using System.Linq;
using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Managers;
using System.Net.Http;
using InterfaceGraphique.CommunicationInterface.RestInterface;
using System.Collections.ObjectModel;
using InterfaceGraphique.Controls.WPF.Chat.Channel;

namespace InterfaceGraphique.Controls.WPF.Friends
{
    public class FriendListItemViewModel : ViewModelBase
    {
        #region Private Properties
        private bool currentFriend;
        private bool addingFriend;
        private bool requestedFriend;
        private UserEntity userEntity;
        #endregion

        #region Public Properties
        public UserEntity UserEntity
        {
            get { return userEntity; }
            set { userEntity = value; }
        }

        public GameRequestManager GameRequestManager { get; }

        public int Id
        {
            get => UserEntity.Id;
            set
            {
                UserEntity.Id = value;
                //this.OnPropertyChanged();
            }
        }

        public string Username
        {
            get => UserEntity.Username;
            set
            {
                UserEntity.Username = value;
                //this.OnPropertyChanged();
            }
        }

        public string ProfilePicture
        {
            get => UserEntity.Profile;
            set
            {
                UserEntity.Profile = value;
               // this.OnPropertyChanged();
            }
        }
        
        public bool IsSelected
        {
            get => UserEntity.IsSelected;
            set
            {
                UserEntity.IsSelected = value;
                //this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(CurrentFriend));
                this.OnPropertyChanged(nameof(AddingFriend));
                this.OnPropertyChanged(nameof(RequestedFriend));
            }
        }

        public bool CurrentFriend
        {
            get => (UserEntity.IsSelected && currentFriend);
            set
            {
                currentFriend = value;
                //this.OnPropertyChanged();
            }
        }

        public bool AddingFriend
        {
            get => (UserEntity.IsSelected && addingFriend);
            set
            {
                addingFriend = value;
                //this.OnPropertyChanged();
            }
        }

        public bool RequestedFriend
        {
            get => (UserEntity.IsSelected && requestedFriend);
            set
            {
                requestedFriend = value;
                //this.OnPropertyChanged();
            }
        }
        #endregion

        #region Constructor
        public FriendListItemViewModel(UserEntity userEntity, GameRequestManager gameRequestManager)
        {
            UserEntity = userEntity;
            GameRequestManager = gameRequestManager;
        }
        #endregion

        #region Commands
        private ICommand clickFriendCommand;
        public ICommand ClickFriendCommand
        {
            get
            {
                if (clickFriendCommand == null)
                {
                    clickFriendCommand = new RelayCommand(ClickFriend);
                }
                return clickFriendCommand;
            }
        }

        private ICommand goToProfileCommand;
        public ICommand GoToProfileCommand
        {
            get
            {
                if (goToProfileCommand == null)
                {
                    goToProfileCommand = new RelayCommandAsync(GoToProfile);
                }
                return goToProfileCommand;
            }
        }

        private ICommand playGameCommand;
        public ICommand PlayGameCommand
        {
            get
            {
                if (playGameCommand == null)
                {
                    playGameCommand = new RelayCommandAsync(PlayGame);
                }
                return playGameCommand;
            }
        }

        private ICommand chatWithCommand;
        public ICommand ChatWithCommand
        {
            get
            {
                if (chatWithCommand == null)
                {
                    chatWithCommand = new RelayCommandAsync(ChatWith);
                }
                return chatWithCommand;
            }
        }

        private ICommand removeFriendCommand;
        public ICommand RemoveFriendCommand
        {
            get
            {
                return removeFriendCommand ??
                       (removeFriendCommand = new RelayCommandAsync(RemoveFriend));
            }
        }

        private ICommand sendFriendRequestCommand;
        public ICommand SendFriendRequestCommand
        {
            get
            {
                return sendFriendRequestCommand ??
                       (sendFriendRequestCommand = new RelayCommandAsync(SendFriendRequest));
            }
        }

        private ICommand acceptFriendRequestCommand;
        public ICommand AcceptFriendRequestCommand
        {
            get
            {
                return acceptFriendRequestCommand ??
                       (acceptFriendRequestCommand = new RelayCommandAsync(AcceptFriendRequest));
            }
        }

        private ICommand refuseFriendRequestCommand;
        public ICommand RefuseFriendRequestCommand
        {
            get
            {
                return refuseFriendRequestCommand ??
                       (refuseFriendRequestCommand = new RelayCommandAsync(RefuseFriendRequest));
            }
        }

        private ICommand mouseOverCommand;
        public ICommand MouseOverCommand
        {
            get
            {
                if (mouseOverCommand == null)
                {
                    mouseOverCommand = new RelayCommand(MouseOverFriend);
                }
                return mouseOverCommand;
            }
        }
        #endregion

        #region Command Methods
        public void ClickFriend()
        {
            foreach (var friend in Program.unityContainer.Resolve<FriendListViewModel>().FriendList)
            {
                friend.IsSelected = false;
            }
            foreach (var friendToAdd in Program.unityContainer.Resolve<AddFriendListViewModel>().Items)
            {
                friendToAdd.IsSelected = false;
            }
            foreach (var friendToAdd in Program.unityContainer.Resolve<FriendRequestListViewModel>().Items)
            {
                friendToAdd.IsSelected = false;
            }
            IsSelected = true;
        }
        public void MouseOverFriend()
        {
            IsSelected = false;
        }
        public async Task GoToProfile()
        {
            System.Diagnostics.Debug.WriteLine("Go to profile of : " + Username + " with id: " + Id);
        }

        public async Task PlayGame()
        {
            await GameRequestManager.SendGameRequest(Id);
            System.Diagnostics.Debug.WriteLine("Play against: " + Username + " with id: " + Id);
        }

        public async Task ChatWith()
        {
            await Program.unityContainer.Resolve<ChannelViewModel>().CreatePrivateChannel(Username, Id);
            System.Diagnostics.Debug.WriteLine("Chat with: " + Username + " with id: " + Id);
        }

        private async Task RemoveFriend()
        {
            UserEntity uE = Program.unityContainer.Resolve<FriendListViewModel>().FriendList.Where(x => x.Username == Username).First().UserEntity;
            await Program.unityContainer.Resolve<FriendsHub>().RemoveFriend(uE);
        }

        private async Task SendFriendRequest()
        {
            HttpResponseMessage response = await Program.client.GetAsync("api/user/u/" + Username);
            UserEntity friend = await HttpResponseParser.ParseResponse<UserEntity>(response);
            await Program.unityContainer.Resolve<FriendsHub>().SendFriendRequest(friend);
            var item = Program.unityContainer.Resolve<AddFriendListViewModel>().Items;
            //Retire de notre liste de personnes ajoutables la personne qu'on vien d'envoyer une demande d'amis
            item.Remove(item.Single(x => x.Id == friend.Id));
        }

        private async Task AcceptFriendRequest()
        {
            await Program.unityContainer.Resolve<FriendsHub>().AcceptFriendRequest(new FriendRequestEntity { Requestor = new UserEntity { Id = Id }, Friend = new UserEntity { Id = User.Instance.UserEntity.Id } });
            var item = Program.unityContainer.Resolve<FriendRequestListViewModel>().Items;
            item.Remove(item.Single(x => x.Id == Id));
        }

        private async Task RefuseFriendRequest()
        {
            if (await Program.unityContainer.Resolve<FriendsHub>().RefuseFriendRequest(new FriendRequestEntity { Requestor = new UserEntity { Id = Id }, Friend = new UserEntity { Id = User.Instance.UserEntity.Id } }))
            {
                var item = Program.unityContainer.Resolve<FriendRequestListViewModel>().Items;
                item.Remove(item.Single(x => x.Id == Id));
                var friendsToAdd = Program.unityContainer.Resolve<AddFriendListViewModel>().Items;
                friendsToAdd.Add(new UserEntity { Id = Id, Username = Username, Profile = ProfilePicture, IsSelected = false });
                Program.unityContainer.Resolve<AddFriendListViewModel>().OnPropertyChanged("Items");
            }
        }
        #endregion

        #region Overwritten Methods
        public override void InitializeViewModel()
        {
            // throw new NotImplementedException();
        }
        #endregion
    }
}
