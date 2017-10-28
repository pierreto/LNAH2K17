using InterfaceGraphique.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InterfaceGraphique.Controls.WPF.Chat.Channel
{
    public class ChatListItemViewModel : ViewModelBase
    {
        private ChannelEntity channelEntity;
        public ChannelEntity ChannelEntity
        {
            get { return channelEntity; }
            set { channelEntity = value; }
        }
        public ChatListItemViewModel(ChannelEntity channelEntity)
        {
            ChannelEntity = channelEntity;
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

        private async Task OpenChannel()
        {
            ActiveChannel.Instance.ChannelEntity = ChannelEntity;
            IsSelected = true;
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
        public bool NewContentAvailable { get; set; }

        public override void InitializeViewModel()
        {
            // throw new NotImplementedException();
        }
    }
}
