using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InterfaceGraphique.CommunicationInterface;

namespace InterfaceGraphique.Menus
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            InitializeEvents();

        }
        public void InitializeOpenGlPanel()
        {
            Program.FormManager.SizeChanged += new EventHandler(WindowSizeChanged);

        }
        public void MettreAJour(double tempsInterAffichage)
        {
            //CurrentForm.MettreAJour(tempsInterAffichage);
        }
        private void InitializeEvents()
        {
            this.LoginButton.Click += async (sender, e) =>
            {

                await runLogin();
        
            };
        }

        private async Task runLogin()
        {
            //Before we actually connect, we check with the server if the login is taken-
            LoginFormMessage loginForm = new LoginFormMessage()
            {
                LoginName = "phlel"
            };
            HttpStatusCode response = await LoginClient.postLoginAsync(loginForm);
            //We initiate the socket connection
            try
            {
                if (response.GetHashCode() == 200)
                {
                    ChatConnection chatConnection = new ChatConnection();
                    chatConnection.EstablishConnection();

                    chatConnection.Send(loginForm);
                    Program.FormManager.CurrentForm = Program.MainMenu;


                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
        public void UnsuscribeEventHandlers()
        {
            Program.FormManager.SizeChanged -= new EventHandler(WindowSizeChanged);
        }
        private void WindowSizeChanged(object sender, EventArgs e)
        {
            this.Size = new Size(Program.FormManager.ClientSize.Width, Program.FormManager.ClientSize.Height);
        }
    }

    public class LoginFormMessage
    {
        private string loginName;

        public string LoginName
        {
            get => loginName;
            set => loginName = value;
        }
    }
}
