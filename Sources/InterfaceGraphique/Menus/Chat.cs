using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InterfaceGraphique.CommunicationInterface;

namespace InterfaceGraphique.Menus
{
    public partial class Chat : Form
    {
        private const string rtfStart="{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang1033{\\fonttbl{\\f0\\fswiss\\fcharset0 Arial;}{\\f1\\fswiss\\fprq2\\fcharset0 Arial;}}{\\colortbl ;\\red0\\green0\\blue128;\\red0\\green128\\blue0;}\\viewkind4\\uc1";
        public Chat()
        {
            InitializeComponent();
            InitializeChatSocket();
        
        }

        private void InitializeChatSocket()
        {
            ChatConnection chatConnection = new ChatConnection();
            chatConnection.EstablishConnection();
            ChatMessage chatMessage = new ChatMessage()
            {
                Sender = "1234",
                MessageValue = "allo"
                
            };
            chatConnection.Send(chatMessage);
            //rtfStart += chatMessage.MessageValue;
            string rtfMsgEncStart = "\\pard\\cf1\\b0\\f1 ";//Code RTF
            string rtfMsgContent = "\\cf2 ";//code RTF
            string formattedMsg = rtfMsgEncStart + chatMessage.Sender + " a écrit:" + rtfMsgContent +
                                  chatMessage.MessageValue + "\\par";
            this.chatViewRichTextBox.Rtf = rtfStart + formattedMsg;
            this.BringToFront();
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
            //CurrentForm.MettreAJour(tempsInterAffichage);
        }
    }
}
