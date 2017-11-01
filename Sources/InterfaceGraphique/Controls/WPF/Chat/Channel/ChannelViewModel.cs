﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Unity;
using InterfaceGraphique.Entities;
using InterfaceGraphique.CommunicationInterface;
using System.Text.RegularExpressions;

namespace InterfaceGraphique.Controls.WPF.Chat.Channel
{
    public class ChannelViewModel : ViewModelBase
    {
        #region Private Properties
        private ChatHub chatHub;

        private string name;

        private bool isOpenAdd;

        private bool isOpenOptions;

        private string channelErrMsg;
        #endregion

        #region Public Properties
        public string Name
        {
            get { return name; }
            set
            {
                if (name != value && value != "")
                {
                    ChannelErrMsg = "";
                    name = value;
                    this.OnPropertyChanged();
                }
                else if (value == "")
                {
                    name = value;
                    this.OnPropertyChanged();
                }
            }
        }

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

        public string ChannelErrMsg
        {
            get { return channelErrMsg; }
            set { channelErrMsg = value; OnPropertyChanged(nameof(ChannelErrMsg)); }
        }
        // This will determine if no channel is selected (After Delete)
        public bool ChannelSelected
        {
            get
            {
                ChannelEntity cE = ActiveChannel.Instance.ChannelEntity;
                if (Program.unityContainer.Resolve<ChatViewModel>().MainChannel == cE)
                {
                    //Disable user from deleting main channel
                    IsOpenOptions = false;
                    return false;
                }
                return cE != null;
            }
        }
        #endregion

        #region Constructor
        public ChannelViewModel(ChatHub chatHub)
        {
            this.chatHub = chatHub;
        }
        #endregion

        #region Commands
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
        #endregion

        #region Command Methods
        private void ToggleAddPopup()
        {
            if (IsOpenAdd)
            {
                IsOpenAdd = false;
            }
            else
            {
                IsOpenAdd = true;
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

        private async Task CreateChannel()
        {
            //Valider le nom
            if (!ValidChannelName()) return;
            //return si nom invalide

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

            ChannelErrMsg = await chatHub.CreateChannel(cE);
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
            }
            else
            {
                ActiveChannel.Instance.ChannelEntity = null;
            }

            //Refresh the possibility to hide the options button
            OnPropertyChanged("ChannelSelected");
            IsOpenOptions = false;
        }

        public async Task JoinChannel()
        {
            ChannelEntity cE = await chatHub.JoinChannel("Secondaire");
            Program.unityContainer.Resolve<ChatListViewModel>().Items.Add(new ChatListItemViewModel(cE));
            Program.unityContainer.Resolve<ChatListViewModel>().OnPropertyChanged("Items");
        }
        #endregion

        #region Private Methods
        private bool ValidChannelName()
        {
            bool valid = true;
            Regex rgx = new Regex(@"^[a-zA-Z0-9_.-]*$");
            if (Name == "" || Name == null)
            {
                ChannelErrMsg = "Nom requis";
                valid = false;
            }
            else if (!rgx.IsMatch(Name))
            {
                ChannelErrMsg = "Nom invalide";
                valid = false;
            }
            return valid;
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
