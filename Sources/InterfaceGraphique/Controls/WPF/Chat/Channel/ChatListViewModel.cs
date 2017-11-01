using InterfaceGraphique.Entities;
using System.Collections.ObjectModel;
using Microsoft.Practices.Unity;

namespace InterfaceGraphique.Controls.WPF.Chat.Channel
{
    public class ChatListViewModel : ViewModelBase
    {
        #region Private Properties
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
        #endregion

        public override void InitializeViewModel()
        {
            //  throw new NotImplementedException();
        }
    }
}
