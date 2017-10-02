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

namespace InterfaceGraphique.Controls.WPF.Chat
{
    public class ChatViewModel : ViewModelBase
    {
        private ChatHub chatHub;
        private string username;
        private ObservableCollection<ChannelEntity> channels;
        private ObservableCollection<ChatMessage> messages;
        private ChannelEntity mainChannel;
        private ChannelEntity currentChannel;
        private TaskFactory ctxTaskFactory;


        private string messageTextBox;


        public ChatViewModel(ChatHub chatHub)
        {

            ctxTaskFactory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());

            // Finally we move from the login page to the main menu:
            this.channels = new ObservableCollection<ChannelEntity>();
            this.mainChannel = new ChannelEntity(new ObservableCollection<ChatMessage>());
            this.messages=new ObservableCollection<ChatMessage>();

            this.chatHub = chatHub;
            chatHub.NewMessage += NewMessage;
            this.currentChannel = mainChannel;

            this.Channels.Add(new ChannelEntity(new ObservableCollection<ChatMessage>())
            {
                Name = "Main"
            });

            
           // Task.Run(() => this.Authenticate()).Wait();


        }

        public void UnsuscribeEventHandlers()
        {
            Program.FormManager.SizeChanged -= new EventHandler(WindowSizeChanged);
        }
        private void WindowSizeChanged(object sender, EventArgs e)
        {
            //this.Size = new Size(Program.FormManager.ClientSize.Width, Program.FormManager.ClientSize.Height);
        }
        public void InitializeOpenGlPanel()
        {
            Program.FormManager.SizeChanged += new EventHandler(WindowSizeChanged);

        }
        public void MettreAJour(double tempsInterAffichage)
        {
        }

        public void Logout()
        {
           this.chatHub.Logout();
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
            });
            this.MessageTextBox = "";
        }

        private ICommand enterKeyCommand;
        public ICommand EnterKeyCommand
        {
            get
            {
                if (enterKeyCommand == null)
                {
                    enterKeyCommand = new RelayCommandAsync(SendMessage, (o) => CanSendMessage());

                }
                return enterKeyCommand;
            }
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
            }
        }


        #region "Event handlers"
        private void NewMessage(ChatMessage message)
        {
            ctxTaskFactory.StartNew(() =>
            {
                this.messages.Add(message);
            }).Wait();     
        }
        #endregion

        public ObservableCollection<ChatMessage> Messages
        {
            get => messages;
            set => messages = value;
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

        public ObservableCollection<ChannelEntity> Channels
        {
            get => channels;
            set
            {
                this.channels = value;
                this.OnPropertyChanged();
            }
        }

        public ChannelEntity CurrentChannel1
        {
            get => currentChannel;
            set => currentChannel = value;
        }
    }


}
