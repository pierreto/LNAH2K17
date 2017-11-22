using InterfaceGraphique.Entities;
using System.Collections.ObjectModel;
using Microsoft.Practices.Unity;
using InterfaceGraphique.CommunicationInterface;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Data;

namespace InterfaceGraphique.Controls.WPF.Chat.Channel
{
    public class ChatListViewModel : ViewModelBase
    {
        #region Private Properties
        private ChatHub chatHub;
        private TaskFactory ctxTaskFactory;
        private ObservableCollection<ChatListItemViewModel> items;
        #endregion

        #region Public Properties
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
        #endregion

        #region Constructor
        public ChatListViewModel(ChatHub chatHub)
        {
            ctxTaskFactory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());
            this.chatHub = chatHub;
            chatHub.NewPrivateChannel += NewPrivateChannel;
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
        #endregion

        private void NewPrivateChannel(string othersName, int othersId)
        {
            ctxTaskFactory.StartNew(() =>
            {
                this.Items.Add(new ChatListItemViewModel(new ChannelEntity { Name = othersName, PrivateUserId = othersId, IsPrivate = true }));
            }).Wait();
        }

        #region Overwritten Methods
        public override void InitializeViewModel()
        {
            //  throw new NotImplementedException();
        }
        #endregion
    }
}
