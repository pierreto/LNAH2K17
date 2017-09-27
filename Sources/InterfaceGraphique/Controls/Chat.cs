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

        private static readonly string rtfStart = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang1033{\\fonttbl{\\f0\\fswiss\\fcharset0 Arial;}{\\f1\\fswiss\\fprq2\\fcharset0 Arial;}}{\\colortbl ;\\red0\\green0\\blue128;\\red0\\green128\\blue0;}\\viewkind4\\uc1";

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
                string rtfMsgEncStart = "\\pard\\cf1\\b0\\f1 ";//Code RTF
                string rtfMsgContent = "\\cf2 ";//code RTF
                string formattedMsg = rtfMsgEncStart + message.Sender + " a écrit:" + rtfMsgContent +
                                      message.MessageValue + "\\par";
                this.chatViewRichTextBox.Rtf = rtfStart + this.chatViewRichTextBox.Rtf + formattedMsg;
                this.BringToFront();
                this.chatViewRichTextBox.ReadOnly = true;

                // set the current caret position to the end
                this.chatViewRichTextBox.SelectionStart = this.chatViewRichTextBox.Text.Length;
                // scroll it automatically
                this.chatViewRichTextBox.ScrollToCaret();

            }

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
                    MessageValue = InputTextBox.Text

                });
                InputTextBox.Text = "";
            }
        }
    }
}
