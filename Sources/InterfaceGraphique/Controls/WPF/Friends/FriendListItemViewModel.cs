using InterfaceGraphique.Entities;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Unity;
using InterfaceGraphique.Controls.WPF.Friends;
using System.Linq;
using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Managers;

namespace InterfaceGraphique.Controls.WPF.Friends
{
    public class FriendListItemViewModel : ViewModelBase
    {
        #region Private Properties
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
                this.OnPropertyChanged();
            }
        }

        public string Username
        {
            get => UserEntity.Username;
            set
            {
                UserEntity.Username = value;
                this.OnPropertyChanged();
            }
        }

        public string ProfilePicture
        {
            get => UserEntity.Profile;
            set
            {
                UserEntity.Profile = value;
                this.OnPropertyChanged();
            }
        }

        public bool IsSelected
        {
            get => UserEntity.IsSelected;
            set
            {
                UserEntity.IsSelected = value;
                this.OnPropertyChanged();
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
        #endregion

        #region Command Methods
        public void ClickFriend()
        {
            foreach (var friend in Program.unityContainer.Resolve<FriendListViewModel>().FriendList)
            {
                friend.IsSelected = false;
            }
            IsSelected = true;
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
            System.Diagnostics.Debug.WriteLine("Chat with: " + Username + " with id: " + Id);
        }

        private async Task RemoveFriend()
        {
            UserEntity uE = Program.unityContainer.Resolve<FriendListViewModel>().FriendList.Where(x => x.Username == Username).First().UserEntity;
            await Program.unityContainer.Resolve<FriendsHub>().RemoveFriend(uE);
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
