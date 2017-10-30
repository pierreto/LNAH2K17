using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Unity;
using InterfaceGraphique.Entities;
using InterfaceGraphique.CommunicationInterface;

namespace InterfaceGraphique.Controls.WPF.Chat.Channel
{
    public class ChannelViewModel : ViewModelBase
    {

        private ChatHub chatHub;
        public ChannelViewModel(ChatHub chatHub)
        {
            this.chatHub = chatHub;
        }

        private ICommand openAddPopupCommand;
        public ICommand OpenAddPopupCommand
        {
            get
            {
                if (openAddPopupCommand == null)
                {
                    openAddPopupCommand = new RelayCommandAsync(ToggleAddPopup);
                }
                return openAddPopupCommand;
            }
        }

        private async Task ToggleAddPopup()
        {
            if (IsOpenAdd)
            {
                IsOpenAdd = false;
                Program.unityContainer.Resolve<ChatViewModel>().Opacity = 1.0f;
            } else
            {
                IsOpenAdd = true;
                Program.unityContainer.Resolve<ChatViewModel>().Opacity = 0.2f;
            }
            Program.unityContainer.Resolve<ChatViewModel>().OnPropertyChanged("Opacity");
        }

        private ICommand openOptionsPopupCommand;
        public ICommand OpenOptionsPopupCommand
        {
            get
            {
                if (openOptionsPopupCommand == null)
                {
                    openOptionsPopupCommand = new RelayCommandAsync(ToggleOptionsPopup);
                }
                return openOptionsPopupCommand;
            }
        }

        private async Task ToggleOptionsPopup()
        {
            if (IsOpenOptions)
            {
                IsOpenOptions = false;
                Program.unityContainer.Resolve<ChatViewModel>().Opacity = 1.0f;
            }
            else
            {
                IsOpenOptions = true;
                Program.unityContainer.Resolve<ChatViewModel>().Opacity = 0.2f;
            }
            Program.unityContainer.Resolve<ChatViewModel>().OnPropertyChanged("Opacity");
        }

        private ICommand createChannelCommand;
        public ICommand CreateChannelCommand
        {
            get
            {
                if (createChannelCommand == null)
                {
                    createChannelCommand = new RelayCommandAsync(CreateChannel);
                }
                return createChannelCommand;
            }
        }

        private async Task CreateChannel()
        {
            ChannelEntity cE = new ChannelEntity() { Name = Name };
            ChatListItemViewModel clivm = new ChatListItemViewModel(cE);
            Program.unityContainer.Resolve<ChatListViewModel>().Items.Add(clivm);
            OnPropertyChanged("Items");
            ToggleAddPopup();
            Name = "";

            //Make sure the previously selected channel is unselected
            foreach (var item in Program.unityContainer.Resolve<ChatListViewModel>().Items)
            {
                item.IsSelected = false;
            }

            //Set to current channel
            var clivmList = Program.unityContainer.Resolve<ChatListViewModel>().Items;
            ActiveChannel.Instance.ChannelEntity = clivmList.Where(s => s.ChannelEntity == cE).First().ChannelEntity;
            clivm.IsSelected = true;
            OnPropertyChanged("ChannelSelected");

            chatHub.CreateChannel(cE);
        }

        private ICommand deleteChannelCommand;
        public ICommand DeleteChannelCommand
        {
            get
            {
                if (deleteChannelCommand == null)
                {
                    deleteChannelCommand = new RelayCommandAsync(DeleteChannel);
                }
                return deleteChannelCommand;
            }
        }

        private async Task DeleteChannel()
        {
            var clivm = Program.unityContainer.Resolve<ChatListViewModel>().Items;
            //Remove the selected ChatListItemViewModel containing the current channel

            clivm.Remove(clivm.Single(s => s.ChannelEntity == ActiveChannel.Instance.ChannelEntity));
            OnPropertyChanged("Items");
            chatHub.LeaveRoom(ActiveChannel.Instance.ChannelEntity.Name);
            //If possible, set the next channel to the current channel
            if (clivm.Any())
            {
                ActiveChannel.Instance.ChannelEntity = clivm.First().ChannelEntity;
                clivm.First().IsSelected = true;
                Program.unityContainer.Resolve<ChatListItemViewModel>().OnPropertyChanged("IsSelected");
            } else
            {
                ActiveChannel.Instance.ChannelEntity = null;
            }
            
            //Refresh the possibility to hide the options button
            OnPropertyChanged("ChannelSelected");
            ToggleOptionsPopup();
        }

        //Name of the channel
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (name == value) return;
                name = value;
                OnPropertyChanged("Name");
            }
        }

        private bool isOpenAdd;
        public bool IsOpenAdd
        {
            get { return isOpenAdd; }
            set
            {
                if (isOpenAdd == value) return;
                isOpenAdd = value;
                OnPropertyChanged("IsOpenAdd");
            }
        }

        private bool isOpenOptions;
        public bool IsOpenOptions
        {
            get { return isOpenOptions; }
            set
            {
                if (isOpenOptions == value) return;
                isOpenOptions = value;
                OnPropertyChanged("IsOpenOptions");
            }
        }

        // This will determine if no channel is selected (After Delete)
        public bool ChannelSelected
        {
            get
            {
                ChannelEntity cE = ActiveChannel.Instance.ChannelEntity;
                if(Program.unityContainer.Resolve<ChatViewModel>().MainChannel == cE)
                {
                    return false;
                }
                return cE != null;
            }
        }

        public override void InitializeViewModel()
        {
            //throw new NotImplementedException();
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

        public async Task JoinChannel()
        {
            ChannelEntity cE = await chatHub.JoinChannel("Secondaire");
            Program.unityContainer.Resolve<ChatListViewModel>().Items.Add(new ChatListItemViewModel(cE));
            Program.unityContainer.Resolve<ChatListViewModel>().OnPropertyChanged("Items");
        }
    }
}
