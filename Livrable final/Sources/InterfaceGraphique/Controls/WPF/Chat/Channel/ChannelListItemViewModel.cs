using InterfaceGraphique.Entities;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Unity;

namespace InterfaceGraphique.Controls.WPF.Chat.Channel
{
    public class ChannelListItemViewModel : ViewModelBase
    {
        #region Private Properties
        private ChannelEntity channelEntity;
        private bool newContentAvailable;
        #endregion

        #region Public Properties
        public ChannelEntity ChannelEntity
        {
            get { return channelEntity; }
            set { channelEntity = value; }
        }

        public string Name
        {
            get => ChannelEntity.Name;
            set
            {
                ChannelEntity.Name = value;
                this.OnPropertyChanged();
            }
        }

        public bool IsJoinable
        {
            get => ChannelEntity.IsJoinable && IsSelected;
            set
            {
                ChannelEntity.IsJoinable = value;
                this.OnPropertyChanged();
            }
        }

        public bool IsSelected
        {
            get => ChannelEntity.IsSelected;
            set
            {
                ChannelEntity.IsSelected = value;
                this.OnPropertyChanged();
            }
        }

        public bool IsPrivate
        {
            get => ChannelEntity.IsPrivate;
            set
            {
                ChannelEntity.IsPrivate = value;
                this.OnPropertyChanged();
            }
        }

        public string Profile
        {
            get => ChannelEntity.Profile;
            set
            {
                ChannelEntity.Profile = value;
                this.OnPropertyChanged();
            }
        }

        public bool NewContentAvailable
        {
            get { return newContentAvailable; }
            set
            {
                newContentAvailable = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Constructor
        public ChannelListItemViewModel(ChannelEntity channelEntity)
        {
            System.Diagnostics.Debug.WriteLine(channelEntity.Name);
            ChannelEntity = channelEntity;
        }
        #endregion

        #region Commands
        private ICommand openChannelCommand;
        public ICommand OpenChannelCommand
        {
            get
            {
                if (openChannelCommand == null)
                {
                    openChannelCommand = new RelayCommand(OpenChannel);
                }
                return openChannelCommand;
            }
        }

        private ICommand joinChannelCommand;
        public ICommand JoinChannelCommand
        {
            get
            {
                if (joinChannelCommand == null)
                {
                    joinChannelCommand = new RelayCommandAsync(JoinChannel);
                }
                return joinChannelCommand;
            }
        }
        private ICommand mouseLeaveCommand;
        public ICommand MouseLeaveCommand
        {
            get
            {
                if (mouseLeaveCommand == null)
                {
                    mouseLeaveCommand = new RelayCommand(MouseLeave);
                }
                return mouseLeaveCommand;
            }
        }
        private ICommand selectChannelCommand;
        public ICommand SelectChannelCommand
        {
            get
            {
                if (selectChannelCommand == null)
                {
                    selectChannelCommand = new RelayCommand(SelectChannel);
                }
                return selectChannelCommand;
            }
        }
        #endregion

        #region Command Methods
        public void MouseLeave()
        {
            if (ChannelEntity.IsJoinable)
            {
                IsSelected = false;
                OnPropertyChanged(nameof(IsJoinable));
            }
        }

        public void OpenChannel()
        {
            if (ChannelEntity.IsJoinable)
            {
                IsSelected = true;
                OnPropertyChanged(nameof(IsJoinable));
            }
        }

        public void SelectChannel()
        {
            if (!ChannelEntity.IsJoinable)
            {
                foreach (var item in Program.unityContainer.Resolve<ChatListViewModel>().Items)
                {
                    item.IsSelected = false;
                }

                IsSelected = true;
                ActiveChannel.Instance.ChannelEntity = ChannelEntity;
                NewContentAvailable = false;
                Program.unityContainer.Resolve<ChannelViewModel>().OnPropertyChanged("ChannelSelected");
            }
        }
        public async Task JoinChannel()
        {
            System.Diagnostics.Debug.WriteLine("Channel Namedfsadaddasdasdsa: " + Name);
            await Program.unityContainer.Resolve<JoinChannelViewModel>().JoinChannel(Name);

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
