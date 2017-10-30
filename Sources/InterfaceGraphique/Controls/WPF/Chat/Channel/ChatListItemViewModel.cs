using InterfaceGraphique.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Unity;


namespace InterfaceGraphique.Controls.WPF.Chat.Channel
{
    public class ChatListItemViewModel : ViewModelBase
    {
        public ChatListItemViewModel(ChannelEntity channelEntity)
        {
            ChannelEntity = channelEntity;
        }

        private ChannelEntity channelEntity;
        public ChannelEntity ChannelEntity
        {
            get { return channelEntity; }
            set { channelEntity = value; }
        }

        private ICommand openChannelCommand;
        public ICommand OpenChannelCommand
        {
            get
            {
                if (openChannelCommand == null)
                {
                    openChannelCommand = new RelayCommandAsync(OpenChannel);
                }
                return openChannelCommand;
            }
        }

        public async Task OpenChannel()
        {
            foreach(var item in Program.unityContainer.Resolve<ChatListViewModel>().Items)
            {
                item.IsSelected = false;
            }
            ActiveChannel.Instance.ChannelEntity = ChannelEntity;
            IsSelected = true;
            NewContentAvailable = false;
            Program.unityContainer.Resolve<ChannelViewModel>().OnPropertyChanged("ChannelSelected");
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

        private bool newContentAvailable;
        public bool NewContentAvailable
        {
            get { return newContentAvailable; }
            set
            {
                newContentAvailable = value;
                OnPropertyChanged();
            }
        }


        public override void InitializeViewModel()
        {
            // throw new NotImplementedException();
        }
    }
}
