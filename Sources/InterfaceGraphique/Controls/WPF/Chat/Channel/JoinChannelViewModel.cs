using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using System.Windows.Input;
using InterfaceGraphique.Entities;
using InterfaceGraphique.CommunicationInterface;

namespace InterfaceGraphique.Controls.WPF.Chat.Channel
{
    public class JoinChannelViewModel : ViewModelBase
    {
        #region Private Properties
        private ChatHub chatHub;
        private string channelName;
        #endregion

        #region Public Properties
        public string ChannelName
        {
            get
            {
                return channelName;
            }
            set
            {
                channelName = value;
                OnPropertyChanged(nameof(ChannelName));
                Program.unityContainer.Resolve<JoinChannelListViewModel>().ItemsView.Refresh();
            }
        }
        #endregion

        #region Constructor
        public JoinChannelViewModel(ChatHub chatHub)
        {
            channelName = "";
            this.chatHub = chatHub;
        }
        #endregion

        #region Commands
        private ICommand joinChannelCommand;
        public ICommand JoinChannelCommand
        {
            get
            {
                if (joinChannelCommand == null)
                {
                    joinChannelCommand = new RelayCommandAsync(JoinChannel, (o) => CanJoin());
                }
                return joinChannelCommand;
            }
        }
        #endregion

        #region Command Methods
        public async Task JoinChannel()
        {
            ChannelEntity cE = await chatHub.JoinChannel(ActiveChannel.Instance.JoinChannelEntity.Name);
            Program.unityContainer.Resolve<ChannelViewModel>().ToggleJoinChannel();
            Program.unityContainer.Resolve<ChatListViewModel>().Items.Add(new ChatListItemViewModel(cE));
            Program.unityContainer.Resolve<ChannelViewModel>().SetAsCurrentChannel(cE);
            Program.unityContainer.Resolve<JoinChannelListViewModel>().Items.Remove(Program.unityContainer.Resolve<JoinChannelListViewModel>().Items.Single(x => x.Name == cE.Name));
            ActiveChannel.Instance.JoinChannelEntity = null;

        }

        public bool CanJoin()
        {
            return ActiveChannel.Instance.JoinChannelEntity != null;
        }
        #endregion

        #region Overwritten Methods
        public override void InitializeViewModel()
        {
            //throw new NotImplementedException();
        }
        #endregion
    }
}
