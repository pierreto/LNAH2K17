using InterfaceGraphique.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceGraphique.Controls.WPF.Chat.Channel
{
    public class ChatListViewModel : ViewModelBase
    {
        public override void InitializeViewModel()
        {
            //  throw new NotImplementedException();
        }

        private List<ChatListItemViewModel> items;
        public List<ChatListItemViewModel> Items
        {
            get
            {
                return items;
            }
            set
            {
                items = value;
                OnPropertyChanged();
            }
        }
        public ChatListViewModel()
        {
            ChannelEntity cE = new ChannelEntity() { Name = "Principal" };
            ActiveChannel.Instance.ChannelEntity = cE;
            Items = new List<ChatListItemViewModel>
            {
                new ChatListItemViewModel(cE)
                {

                },
                new ChatListItemViewModel(new ChannelEntity(){ Name = "Secondaire"})
                {

                }
            };
        }

        private ObservableCollection<ChannelEntity> channels;
        public ObservableCollection<ChannelEntity> Channels
        {
            get => channels;
            set
            {
                this.channels = value;
                this.OnPropertyChanged();
            }
        }
    }
}
