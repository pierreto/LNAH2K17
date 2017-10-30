using InterfaceGraphique.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Unity;

namespace InterfaceGraphique.Controls.WPF.Chat.Channel
{
    public class ChatListViewModel : ViewModelBase
    {
        public ChatListViewModel()
        {
            ChannelEntity cE = new ChannelEntity() { Name = "Principal", IsSelected = true };
            ActiveChannel.Instance.ChannelEntity = cE;
            Program.unityContainer.Resolve<ChatViewModel>().MainChannel = cE;
            Items = new ObservableCollection<ChatListItemViewModel>
            {
                new ChatListItemViewModel(cE)
                {

                }
            };
        }

        private ObservableCollection<ChatListItemViewModel> items;
        public ObservableCollection<ChatListItemViewModel> Items
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

        public override void InitializeViewModel()
        {
            //  throw new NotImplementedException();
        }
    }
}
