﻿using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Entities;
using Microsoft.Practices.Unity;
using InterfaceGraphique.Controls.WPF.Chat.Channel;
using System.Linq;
using System.Windows;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace InterfaceGraphique.Controls.WPF.Chat
{
    public class ChatViewModel : ViewModelBase
    {
        private ChatHub chatHub;
        private TaskFactory ctxTaskFactory;
        public Window UndockedChat { get; private set; }
        public ChatViewModel(ChatHub chatHub)
        {
            this.chatHub = chatHub;
            docked = true;
            chatHub.NewMessage += NewMessage;
            chatHub.NewMessageFromChannel += NewMessageFromChannel;
            ctxTaskFactory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());
            currentChannel = CurrentChannel;
            ChatTabHeight = 40;
        }

        private void OnUnDockedWindowClosing(object sender, CancelEventArgs e)
        {
            Program.MainMenu.ShowChat();
            Docked = true;
            ChatTabHeight = 40;
        }

        private bool docked;
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

        private ICommand dockCommand;
        public ICommand DockCommand
        {
            get
            {
                if (dockCommand == null)
                {
                    dockCommand = new RelayCommandAsync(UnDock);

                }
                return dockCommand;
            }
        }

        private async Task UnDock()
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

        private bool CanSendMessage()
        {
            return !string.IsNullOrEmpty(MessageTextBox);
        }

        private ChannelEntity currentChannel;
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

        private string messageTextBox;
        public string MessageTextBox
        {
            get => messageTextBox;
            set
            {
                messageTextBox = value;
                OnPropertyChanged("MessageTextBox");
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

        private Visibility collapsed;
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
                OnPropertyChanged(nameof(NotCollapsed));
            }
        }

        public Visibility NotCollapsed
        {
            get
            {
                if (Collapsed == System.Windows.Visibility.Visible)
                {
                    return System.Windows.Visibility.Collapsed;
                }
                else
                {
                    return System.Windows.Visibility.Visible;
                }
            }
        }

        private int chatTabHeight;
        public int ChatTabHeight
        {
            get { return chatTabHeight; }
            set
            {
                chatTabHeight = value;
                OnPropertyChanged(nameof(ChatTabHeight));
            }
        }
        public override void InitializeViewModel()
        {
            //Empty for the moment
        }
    }


}
