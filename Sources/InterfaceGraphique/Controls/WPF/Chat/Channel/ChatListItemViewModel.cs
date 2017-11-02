using InterfaceGraphique.Entities;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Unity;

namespace InterfaceGraphique.Controls.WPF.Chat.Channel
{
    public class ChatListItemViewModel : ViewModelBase
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

        public bool IsSelected
        {
            get => ChannelEntity.IsSelected;
            set
            {
                ChannelEntity.IsSelected = value;
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
        public ChatListItemViewModel(ChannelEntity channelEntity)
        {
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
        #endregion

        #region Command Methods
        public void OpenChannel()
        {
            if (Program.unityContainer.Resolve<ChatViewModel>().JoinMenuOpen)
            {
                foreach (var item in Program.unityContainer.Resolve<JoinChannelListViewModel>().Items)
                {
                    item.IsSelected = false;
                }
                ActiveChannel.Instance.JoinChannelEntity = ChannelEntity;
            }
            else
            {
                foreach (var item in Program.unityContainer.Resolve<ChatListViewModel>().Items)
                {
                    item.IsSelected = false;
                }
                ActiveChannel.Instance.ChannelEntity = ChannelEntity;
                NewContentAvailable = false;
                Program.unityContainer.Resolve<ChannelViewModel>().OnPropertyChanged("ChannelSelected");
            }
            IsSelected = true;
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
