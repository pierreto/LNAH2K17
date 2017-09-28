using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Menus;
using Color = System.Drawing.Color;
using Size = System.Drawing.Size;
using UserControl = System.Windows.Controls.UserControl;

namespace InterfaceGraphique.Controls
{
    /// <summary>
    /// Logique d'interaction pour WPFChatView.xaml
    /// </summary>
    public partial class WPFChatView : UserControl
    {


        private LoginFormMessage loginForm;
        private ChatHub chatHub;
        private Messages messages;

        public WPFChatView()
        {
            InitializeComponent();
            this.chatHub = new ChatHub(UpdateChatBoxDelegate);
            messages = new Messages();
            DataContext = messages;
        }
        private void InitializeEvents()
        {
            this.SendButton.Click += (sender, e) => this.SendMessage(sender, e);
            //this.InputTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(CheckEnterKeyPress);

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

            messages.ListOfItems.Add(new CMessage(message.MessageValue,message.Sender, System.DateTime.Now, true));





        }
        private void CheckEnterKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                SendMessage(sender, e);
                e.Handled = true;

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





        public void Logout()
        {
          //  this.chatHub.Logout(this.username);
        }




    }
}
public class User
{
    public string Name { get; set; }
    public string ID { get; set; }
    public byte[] Photo { get; set; }
}

public class CMessage
{
    public CMessage(string message, string author, DateTime time, bool isOriginNative)
    {
        Message = message;
        Author = author;
        Time = time;
        IsOriginNative = isOriginNative;
    }

    public string Message { get; set; }
    public string Author { get; set; }
    public DateTime Time { get; set; }
    public bool IsOriginNative { get; set; }
}

public class Messages
{
    public IList<CMessage> ListOfItems { get; set; }

    public Messages()
    {
        ListOfItems = new List<CMessage>();
        ListOfItems.Add(new CMessage("asdf","asdfasd",System.DateTime.Now,true));
        //ListOfItems.Add("Two");
       // ListOfItems.Add("Three");
    }

}
