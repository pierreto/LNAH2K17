﻿using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Entities;
using Microsoft.Practices.Unity;
using InterfaceGraphique.Controls.WPF.Chat.Channel;
using System.Linq;
using System.Windows;
using System.ComponentModel;

namespace InterfaceGraphique.Controls.WPF.Chat
{
    public class ChatViewModel : ViewModelBase
    {

        #region Private Properties
        private readonly int CHAT_TAB_HEIGHT = 40;
        private ChatHub chatHub;
        private TaskFactory ctxTaskFactory;
        private bool docked;
        private bool joinMenuOpen;
        private int chatTabHeight;
        private string messageTextBox;
        private ChannelEntity currentChannel;
        private Visibility collapsed;
        #endregion

        #region Public Properties
        public Window UndockedChat { get; private set; }
        public bool Docked
        {
            get
            {
                return docked;
            }
            set
            {
                docked = value;
                OnPropertyChanged(nameof(Docked));
            }
        }
        public bool JoinMenuOpen
        {
            get
            {
                return joinMenuOpen;
            }
            set
            {
                joinMenuOpen = value;
                OnPropertyChanged(nameof(JoinMenuOpen));
            }
        }
        public int ChatTabHeight
        {
            get { return chatTabHeight; }
            set
            {
                chatTabHeight = value;
                OnPropertyChanged(nameof(ChatTabHeight));
            }
        }
        public string MessageTextBox
        {
            get => messageTextBox;
            set
            {
                messageTextBox = value;
                OnPropertyChanged("MessageTextBox");
            }
        }
        public ChannelEntity CurrentChannel
        {
            get => currentChannel;
            set
            {
                if (value == currentChannel)
                {
                    return;
                }
                currentChannel = value;
                OnPropertyChanged("CurrentChannel");
                OnPropertyChanged("Messages");
            }
        }
        public ChannelEntity MainChannel { get; set; }
        public ObservableCollection<ChatMessage> Messages
        {
            get
            {
                if (CurrentChannel != null)
                {
                    return CurrentChannel.Messages;
                }
                return null;
            }
            set
            {
                if (CurrentChannel != null)
                {
                    CurrentChannel.Messages = value;
                    this.OnPropertyChanged();
                }
            }
        }
        public Visibility Collapsed
        {
            get
            {
                return collapsed;
            }
            set
            {
                collapsed = value;
                OnPropertyChanged(nameof(Collapsed));
            }
        }
        #endregion

        #region Constructor
        public ChatViewModel(ChatHub chatHub)
        {
            this.chatHub = chatHub;
            docked = true;
            this.joinMenuOpen = false;
            chatHub.NewMessage += NewMessage;
            chatHub.NewMessageFromChannel += NewMessageFromChannel;
            ctxTaskFactory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());
            currentChannel = CurrentChannel;
            ChatTabHeight = CHAT_TAB_HEIGHT;
        }
        #endregion

        #region Commands
        private ICommand sendMessageCommand;
        public ICommand SendMessageCommand
        {
            get
            {
                if (sendMessageCommand == null)
                {
                    sendMessageCommand = new RelayCommandAsync(SendMessage, (o) => CanSendMessage());

                }
                return sendMessageCommand;
            }
        }

        private ICommand unDockCommand;
        public ICommand UnDockCommand
        {
            get
            {
                if (unDockCommand == null)
                {
                    unDockCommand = new RelayCommand(UnDock);

                }
                return unDockCommand;
            }
        }

        private ICommand minimizeCommand;
        public ICommand MinimizeCommand
        {
            get
            {
                if (minimizeCommand == null)
                {
                    minimizeCommand = new RelayCommand(Minimize);

                }
                return minimizeCommand;
            }
        }
        #endregion

        #region Command Methods
        private async Task SendMessage()
        {
            if (CurrentChannel == MainChannel)
            {
                this.chatHub.SendMessage(new ChatMessage()
                {
                    MessageValue = MessageTextBox,
                    SentByMe = false
                });
            }
            else
            {
                this.chatHub.SendChannel(new ChatMessage()
                {
                    MessageValue = MessageTextBox,
                    SentByMe = false
                }, CurrentChannel.Name);
            }
            MessageTextBox = "";
        }
        private void UnDock()
        {
            Docked = false;
            Program.MainMenu.HideChat();
            UndockedChat = new Window
            {
                Title = "UnDocked",
                Content = new TestChatView()
                {
                    Titre = "UnDocked",
                    DataContext = Program.unityContainer.Resolve<ChatViewModel>(),
                }

            };
            UndockedChat.Closing += this.OnUnDockedWindowClosing;
            System.Windows.Forms.Integration.ElementHost.EnableModelessKeyboardInterop(UndockedChat);
            ChatTabHeight = 0;
            UndockedChat.Show();
        }
        public void Minimize()
        {
            if (Collapsed == System.Windows.Visibility.Visible)
            {
                Collapsed = System.Windows.Visibility.Collapsed;
            }
            else
            {
                Collapsed = System.Windows.Visibility.Visible;
            }
        }
        #endregion

        #region Private Methods
        private void OnUnDockedWindowClosing(object sender, CancelEventArgs e)
        {
            Program.MainMenu.ShowChat();
            Docked = true;
            ChatTabHeight = CHAT_TAB_HEIGHT;
        }
        private bool CanSendMessage()
        {
            return !string.IsNullOrEmpty(MessageTextBox);
        }
        private void NewMessage(ChatMessage message)
        {
            ctxTaskFactory.StartNew(() =>
            {
                //If you are not currently on the channel where the message is being sent, you receive a notification
                if (CurrentChannel != MainChannel)
                {
                    var clivmList = Program.unityContainer.Resolve<ChatListViewModel>().Items;
                    var clivm = clivmList.Where(s => s.ChannelEntity == MainChannel).First();
                    clivm.NewContentAvailable = true;
                    Program.unityContainer.Resolve<ChatListItemViewModel>().OnPropertyChanged("ChannelSelected");
                }
                MainChannel.Messages.Add(message);
            }).Wait();
        }
        private void NewMessageFromChannel(ChatMessage message, ChannelEntity channelEntity)
        {
            var items = Program.unityContainer.Resolve<ChatListViewModel>().Items;
            ChannelEntity cE = items.Where(s => s.ChannelEntity.Name == channelEntity.Name).First().ChannelEntity;
            ctxTaskFactory.StartNew(() =>
            {
                //If you are not currently on the channel where the message is being sent, you receive a notification
                if (CurrentChannel != cE)
                {
                    var clivmList = Program.unityContainer.Resolve<ChatListViewModel>().Items;
                    var clivm = clivmList.Where(s => s.ChannelEntity == cE).First();
                    clivm.NewContentAvailable = true;
                    Program.unityContainer.Resolve<ChatListItemViewModel>().OnPropertyChanged("ChannelSelected");
                }
                cE.Messages.Add(message);
            }).Wait();
        }
        #endregion

        #region Overwritten Methods
        public override void InitializeViewModel()
        {
            //Empty for the moment
        }
        #endregion
    }


}
