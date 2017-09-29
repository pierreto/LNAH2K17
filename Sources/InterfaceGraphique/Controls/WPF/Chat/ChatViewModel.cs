using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Menus;

namespace InterfaceGraphique.Controls.WPF.Chat
{
    class ChatViewModel
    {
        private LoginFormMessage loginForm;
        private ChatHub chatHub;
        private Messages messages;

        private void InitializeEvents()
        {
            //this.SendButton.Click += (sender, e) => this.SendMessage(sender, e);
            //this.InputTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(CheckEnterKeyPress);
            this.chatHub = new ChatHub(UpdateChatBoxDelegate);
            messages = new Messages();
            //DataContext = messages;
        }


        // TODO: retourner un résultat pour savoir si la connexion a échouée ou pas.
        public async Task EstablishConnection(string targetServerIp)
        {
            await this.chatHub.EstablishConnection(targetServerIp);
        }

        public async Task<bool> AuthenticateUser(string username)
        {
            var auth = this.chatHub.AuthenticateUser(username);
            await auth;
            return auth.Result;
        }

        public async Task InitializeChat(LoginFormMessage loginForm)
        {
            await this.chatHub.InitializeChat();

            this.loginForm = loginForm;

            this.chatHub.SendMessage(new ChatMessage()
            {
                MessageValue = "bonjour",
                Sender = "les amis",
                TimeStamp = DateTime.Now
            });

            /*if (!this.IsHandleCreated)
            {
                this.CreateHandle();
            }*/
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

        private void UpdateChatBoxDelegate(ChatMessage message)
        {
            /*
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<ChatMessage>(UpdateChatBoxDelegate), message);
            }
            else
            {
                // This will run on the UI thread
                this.chatViewRichTextBox.ReadOnly = false;
                this.AppendText(message.Sender + " a écrit: ", System.Drawing.Color.Blue);
                this.AppendText(message.MessageValue + "\r\n", System.Drawing.Color.Green);
                this.BringToFront();
                this.chatViewRichTextBox.ReadOnly = true;

                // set the current cursor position to the end
                this.chatViewRichTextBox.SelectionStart = this.chatViewRichTextBox.Text.Length;
                // scroll it automatically
                this.chatViewRichTextBox.ScrollToCaret();

            }*/

            messages.ListOfItems.Add(new ChatMessage(message.MessageValue, message.Sender, System.DateTime.Now));





        }
        private void CheckEnterKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                SendMessage(sender, e);
                e.Handled = true;

            }
        }

     





        public void Logout()
        {
            //  this.chatHub.Logout(this.username);
        }





        private ICommand sendMessageCommand;
        public ICommand SendMessageCommand
        {
            get
            {
                if (sendMessageCommand == null) sendMessageCommand =
                    new RelayCommandAsync(() => SendMessage(), (o) => CanSendMessage());
                return sendMessageCommand;
            }
        }
        private void SendMessage(object sender, EventArgs e)
        {
            /*
            if (InputTextBox.Text.Length > 0)
            {
                this.chatHub.SendMessage(new ChatMessage()
                {
                    Sender = loginForm.LoginName,
                    MessageValue = InputTextBox.Text,
                    TimeStamp = DateTime.Now
                });
                InputTextBox.Text = "";
            }*/
        }
        private bool CanSendMessage()
        {
            return !string.IsNullOrEmpty(Message) && _selectedParticipant != null && _selectedParticipant.IsLoggedIn;
        }
    }


}
