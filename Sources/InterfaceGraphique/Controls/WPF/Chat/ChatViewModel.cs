
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Entities;
using InterfaceGraphique.Exceptions;
using InterfaceGraphique.Menus;
using InterfaceGraphique.Controls.WPF.Chat.Channel;

namespace InterfaceGraphique.Controls.WPF.Chat
{
    public class ChatViewModel : ViewModelBase
    {
        private ChatHub chatHub;
        private ChannelEntity mainChannel;
        private ChannelEntity currentChannel;
        private TaskFactory ctxTaskFactory;


        private string messageTextBox;

        public ChatViewModel(ChatHub chatHub)
        {
            this.chatHub = chatHub;
            chatHub.NewMessage += NewMessage;

            ctxTaskFactory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());
            this.currentChannel = CurrentChannel;
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

        private async Task SendMessage()
        {
            this.chatHub.SendMessage(new ChatMessage()
            {
                MessageValue = this.messageTextBox,
                SentByMe = false
            });
            MessageTextBox = "";
        }

        private bool CanSendMessage()
        {
            return !string.IsNullOrEmpty(MessageTextBox);
        }

        public ChannelEntity MainChannel
        {
            get => mainChannel;
            set => mainChannel = value;
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
                this.OnPropertyChanged();
                //To update messages...
                Messages = Messages;
            }
        }

        private void NewMessage(ChatMessage message)
        {
            ctxTaskFactory.StartNew(() =>
            {
                CurrentChannel.Messages.Add(message);
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

        public string MessageTextBox
        {
            get => this.messageTextBox;
            set
            {
                this.messageTextBox = value;
                this.OnPropertyChanged();
            }
        }

        public override void InitializeViewModel()
        {
            //Empty for the moment
        }

    }


}
