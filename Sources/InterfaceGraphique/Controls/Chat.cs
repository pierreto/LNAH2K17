using System;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using System.Threading.Tasks;
using InterfaceGraphique.CommunicationInterface;

namespace InterfaceGraphique.Menus
{
    public partial class Chat : UserControl
    {
        private LoginFormMessage loginForm;
        private ChatHub chatHub;
        private string username;

        public Chat()
        {
            InitializeComponent();
            InitializeEvents();

            this.chatViewRichTextBox.ReadOnly = true;
            this.chatHub = new ChatHub(UpdateChatBoxDelegate);
        }


        private void InitializeEvents()
        {
            this.SendButton.Click += (sender, e) => this.SendMessage(sender, e);
            this.InputTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(CheckEnterKeyPress);

        }


       // TODO: retourner un résultat pour savoir si la connexion a échouée ou pas.
        public async Task EstablishConnection(string targetServerIp)
        {
            await this.chatHub.EstablishConnection(targetServerIp);
            this.chatHub.test();
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
            this.username = loginForm.LoginName;

            if (!this.IsHandleCreated)
            {
                this.CreateHandle();
            }
        }

        public void UnsuscribeEventHandlers()
        {
            Program.FormManager.SizeChanged -= new EventHandler(WindowSizeChanged);
        }
        private void WindowSizeChanged(object sender, EventArgs e)
        {
            this.Size = new Size(Program.FormManager.ClientSize.Width, Program.FormManager.ClientSize.Height);
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

            if (this.InvokeRequired)
            {
                this.Invoke(new Action<ChatMessage>(UpdateChatBoxDelegate), message);
            }
            else
            {
                // This will run on the UI thread
                this.chatViewRichTextBox.ReadOnly = false;
                this.AppendText(message.TimeStamp.ToLongTimeString() + " ", Color.Black);
                this.AppendText(message.Sender + " a écrit: ", Color.Blue);
                this.AppendText(message.MessageValue + "\r\n", Color.Green);
                this.BringToFront();
                this.chatViewRichTextBox.ReadOnly = true;

                // set the current cursor position to the end
                this.chatViewRichTextBox.SelectionStart = this.chatViewRichTextBox.Text.Length;
                // scroll it automatically
                this.chatViewRichTextBox.ScrollToCaret();

            }

        }

        private  void AppendText(string text, Color color)
        {
            this.chatViewRichTextBox.SelectionStart = this.chatViewRichTextBox.TextLength;
            this.chatViewRichTextBox.SelectionLength = 0;

            this.chatViewRichTextBox.SelectionColor = color;
            this.chatViewRichTextBox.AppendText(text);
            this.chatViewRichTextBox.SelectionColor = this.chatViewRichTextBox.ForeColor;
        }
        
        private void CheckEnterKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                SendMessage(sender,e);
                e.Handled = true;

            }
        }

        private void SendMessage(object sender, EventArgs e)
        {
            if (InputTextBox.Text.Length > 0)
            {
                this.chatHub.SendMessage(new ChatMessage()
                {
                    Sender = loginForm.LoginName,
                    MessageValue = InputTextBox.Text,
                    TimeStamp = DateTime.Now
                });
                InputTextBox.Text = "";
                InputTextBox.Focus();
            }
        }
        public void Logout()
        {
            if (this.chatHub != null)
            {
                this.chatHub.Logout(this.username);
            }
        }
    }
}