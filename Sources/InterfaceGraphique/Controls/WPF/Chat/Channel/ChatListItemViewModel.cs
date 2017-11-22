﻿using InterfaceGraphique.Entities;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Unity;

namespace InterfaceGraphique.Controls.WPF.Chat.Channel
{
    public class ChatListItemViewModel : ViewModelBase
    {
        #region Private Properties
        private ChannelEntity channelEntity;
        private string profile;
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

        public string Profile
        {
            get => profile;
            set
            {
                profile = value;
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
                bool channelJoined = false;
                foreach (var item in Program.unityContainer.Resolve<ChatListViewModel>().Items)
                {
                    if (item.ChannelEntity.Name == ChannelEntity.Name)
                    {
                        channelJoined = true;
                    }
                }
                if (!channelJoined)
                {
                    ActiveChannel.Instance.JoinChannelEntity = ChannelEntity;
                }
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
            System.Diagnostics.Debug.WriteLine(ChannelEntity.Name);
            IsSelected = true;
        }

        public async Task JoinChannel()
        {
            await Program.unityContainer.Resolve<JoinChannelViewModel>().JoinChannel();
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
