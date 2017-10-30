using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Entities;
using Microsoft.Practices.Unity;
using InterfaceGraphique.Controls.WPF.Chat.Channel;
using System.Linq;

namespace InterfaceGraphique.Controls.WPF.Chat
{
    public class ChatViewModel : ViewModelBase
    {
        private ChatHub chatHub;
        private TaskFactory ctxTaskFactory;
        private ChannelEntity mainChannel;
        public ChatViewModel(ChatHub chatHub)
        {
            this.chatHub = chatHub;
            chatHub.NewMessage += NewMessage;
            chatHub.NewMessageFromChannel += NewMessageFromChannel;
            ctxTaskFactory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());
            this.currentChannel = CurrentChannel;
            
            opacity = 1.0f;
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
            if(CurrentChannel == MainChannel)
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
                });
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

        private float opacity;
        public float Opacity
        {
            get
            {
                return opacity;
            }
            set
            {
                if (opacity != value)
                {
                    opacity = value;
                    OnPropertyChanged("Opacity");
                }
            }
        }
        public override void InitializeViewModel()
        {
            //Empty for the moment
        }

    }


}
