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
                    openAddPopupCommand = new RelayCommand(ToggleAddPopup);
                }
                return openAddPopupCommand;
            }
        }

        private void ToggleAddPopup()
        {
            if (IsOpenAdd)
            {
                IsOpenAdd = false;
            } else
            {
                IsOpenAdd = true;
            }
        }

        private ICommand openOptionsPopupCommand;
        public ICommand OpenOptionsPopupCommand
        {
            get
            {
                if (openOptionsPopupCommand == null)
                {
                    openOptionsPopupCommand = new RelayCommand(ToggleOptionsPopup);
                }
                return openOptionsPopupCommand;
            }
        }

        private void ToggleOptionsPopup()
        {
            if (IsOpenOptions)
            {
                IsOpenOptions = false;
            }
            else
            {
                IsOpenOptions = true;
            }
        }

        private ICommand createChannelCommand;
        public ICommand CreateChannelCommand
        {
            get
            {
                if (createChannelCommand == null)
                {
                    createChannelCommand = new RelayCommand(CreateChannel);
                }
                return createChannelCommand;
            }
        }

        private void CreateChannel()
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
                    deleteChannelCommand = new RelayCommand(DeleteChannel);
                }
                return deleteChannelCommand;
            }
        }

        private void DeleteChannel()
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
            IsOpenOptions = false;
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
                    //Disable user from deleting main channel
                    IsOpenOptions = false;
                    return false;
                }
                return cE != null;
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

        public async Task JoinChannel()
        {
            ChannelEntity cE = await chatHub.JoinChannel("Secondaire");
            Program.unityContainer.Resolve<ChatListViewModel>().Items.Add(new ChatListItemViewModel(cE));
            Program.unityContainer.Resolve<ChatListViewModel>().OnPropertyChanged("Items");
        }

        private ICommand closeCreateChannelCommand;
        public ICommand CloseCreateChannelCommand
        {
            get
            {
                if (closeCreateChannelCommand == null)
                {
                    closeCreateChannelCommand = new RelayCommand(ToggleAddPopup);
                }
                return closeCreateChannelCommand;
            }
        }

        private ICommand closeOptionsCommand;
        public ICommand CloseOptionsCommand
        {
            get
            {
                if (closeOptionsCommand == null)
                {
                    closeOptionsCommand = new RelayCommand(ToggleOptionsPopup);
                }
                return closeOptionsCommand;
            }
        }

        public override void InitializeViewModel()
        {
            //throw new NotImplementedException();
        }
    }
}
