using System;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
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

        }


        private void InitializeEvents()
        {
            this.SendButton.Click += (sender, e) =>
            {
                this.chatHub.SendMessage(new ChatMessage()
                {
                    Sender = loginForm.LoginName,
                    MessageValue = InputTextBox.Text

                });
                InputTextBox.Text = "";

            };
        }

        private static readonly string rtfStart = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang1033{\\fonttbl{\\f0\\fswiss\\fcharset0 Arial;}{\\f1\\fswiss\\fprq2\\fcharset0 Arial;}}{\\colortbl ;\\red0\\green0\\blue128;\\red0\\green128\\blue0;}\\viewkind4\\uc1";

        public void InitializeChatSocket(LoginFormMessage loginForm,IPAddress targetServerIp)
        {

            this.loginForm = loginForm;

            if (!this.IsHandleCreated)
            {
                this.CreateHandle();
            }

            chatHub = new ChatHub(targetServerIp, UpdateChatBoxDelegate);
            chatHub.EstablishConnection();
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
                string rtfMsgEncStart = "\\pard\\cf1\\b0\\f1 ";//Code RTF
                string rtfMsgContent = "\\cf2 ";//code RTF
                string formattedMsg = rtfMsgEncStart + message.Sender + " a écrit:" + rtfMsgContent +
                                      message.MessageValue + "\\par";
                this.chatViewRichTextBox.Rtf = rtfStart + this.chatViewRichTextBox.Rtf + formattedMsg;
                this.BringToFront();
            }
   
        }

    }
}
