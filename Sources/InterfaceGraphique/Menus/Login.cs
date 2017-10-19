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
using InterfaceGraphique.Exceptions;
using Microsoft.AspNet.SignalR.Client;

namespace InterfaceGraphique.Menus
{
    public partial class Login : Form
    {
        private readonly string LOCALHOST = "localhost";
        private readonly int MAX_INPUT_LENGTH = 15;
        private ChatHub chatHub;
        private HubManager hubManager;
        public Login(ChatHub chatHub)
        {
            this.chatHub = chatHub;
            InitializeComponent();
            InitializeEvents();
            this.ServerLabel.BackColor = System.Drawing.Color.Transparent;
            this.UsernameLabel.BackColor = System.Drawing.Color.Transparent;
            this.sonicPuckLabel.BackColor = System.Drawing.Color.Transparent;

            this.ServerTextBox.Text = @"localhost";
            this.ServerTextBox.MaxLength = MAX_INPUT_LENGTH;
            this.UsernameTextBox.MaxLength = MAX_INPUT_LENGTH;

        }
        public void InitializeOpenGlPanel()
        {
            Program.FormManager.SizeChanged += new EventHandler(WindowSizeChanged);

        }
        public void MettreAJour(double tempsInterAffichage)
        {
        }
        private void InitializeEvents()
        {
            this.LoginButton.Click += async (sender, e) =>
            {
                await RunLogin();  
            };

            this.UsernameTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(CheckEnterKeyPress);

            this.ServerTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(CheckEnterKeyPress);

        }

        private async Task RunLogin()
        {
            this.LoginButton.Enabled = false;
           
            try
            {
                ValidateUserInput();

                LoginFormMessage loginForm = new LoginFormMessage()
                {
                    LoginName = UsernameTextBox.Text
                };
                this.hubManager = new HubManager();

                // We first initialize the connection with the chat server:
                //await hubManager.EstablishConnection(ServerTextBox.Text,UsernameTextBox.Text );
                // Then we try to authenticate the user with the username he/she gave:
                var authentication = chatHub.AuthenticateUser();
                await authentication;
                if (authentication.Result)
                {
                    Program.client.BaseAddress = new System.Uri("http://" + ServerTextBox.Text+ ":63056/");

                    // We initialize the chat to activate broadcasting from the server:
                    await chatHub.InitializeChat();
                    // Finally we move from the login page to the main menu:
                    Program.FormManager.CurrentForm = Program.MainMenu;
                }
                else
                {
                    UsernameTextBox.Clear();
                    throw new LoginException(@"Ce nom d'utilisateur est déjà pris. Veuillez en choisir un autre.");
                }
                Program.FormManager.CurrentForm = Program.MainMenu;

            }
            catch (LoginException e)
            {
                Console.WriteLine(e);
                MessageBox.Show(
                    e.Message,
                    @"Erreur de champs",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (System.Exception exception)
            {
                //Server Exceptions...
                Console.WriteLine(exception);
                MessageBox.Show(
                    @"La connexion a échoué. L'adresse IP spécifié est peut-être erroné ou bien le serveur est hors-fonction.",
                    @"Erreur de connexion",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                this.LoginButton.Enabled = true;
            }
        }

        private void ValidateUserInput()
        {
            if (!ValidateIP(ServerTextBox.Text) && !LOCALHOST.Equals(ServerTextBox.Text))
            {
                throw new LoginException(@"Le format de l'adresse IP n'est pas valide.");
            }
            if (String.IsNullOrEmpty(UsernameTextBox.Text))
            {
                throw new LoginException(@"Le champ du nom d'usager ne peut être vide.");
            }
            if(UsernameTextBox.Text.Any(c => !(Char.IsLetterOrDigit(c) || c.Equals('_'))))
            {
                throw new LoginException(@"Le nom d'usager peut seulement contenir des chiffre, des lettre et des tirets en bas (_)");
            }
            if (String.IsNullOrEmpty(ServerTextBox.Text))
            {
                throw new LoginException(@"Le champ de l'adresse du serveur ne peut être vide.");
            }
        }

        private bool ValidateIP(string ipString)
        {
            if (String.IsNullOrWhiteSpace(ipString))
            {
                return false;
            }

            string[] splitValues = ipString.Split('.');
            if (splitValues.Length != 4)
            {
                return false;
            }
            return splitValues.All(r => byte.TryParse(r, out byte tempForParsing));
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

        private async void CheckEnterKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                await RunLogin();
                e.Handled = true;
            }

        }

        public void Logout()
        {
            this.hubManager.Logout();
            Program.FormManager.CurrentForm = Program.Login;
        }
    }

    public class LoginFormMessage
    {
        private string loginName;
        private string password;

        public string LoginName
        {
            get => loginName;
            set => loginName = value;
        }

        public string Password
        {
            get => password;
            set => password = value;
        }

    }

}
