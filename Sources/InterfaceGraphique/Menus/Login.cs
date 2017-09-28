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

namespace InterfaceGraphique.Menus
{
    public partial class Login : Form
    {
        private readonly string LOCALHOST = "localhost";
        private readonly int MAX_INPUT_LENGTH = 15;
        public Login()
        {
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
                if (!ValidateIP(ServerTextBox.Text) && !LOCALHOST.Equals(ServerTextBox.Text))
                {
                    throw new LoginException(@"Le format de l'adresse IP n'est pas valide.");
                }
                if (String.IsNullOrEmpty(UsernameTextBox.Text))
                {
                    throw new LoginException(@"Le champ du nom d'usager ne peut être vide.");
                }
                if (String.IsNullOrEmpty(ServerTextBox.Text))
                {
                    throw new LoginException(@"Le champ de l'adresse du serveur ne peut être vide.");
                }

                LoginFormMessage loginForm = new LoginFormMessage()
                {
                    LoginName = UsernameTextBox.Text
                };

                // We first initialize the connection with the chat server:
                await Program.MainMenu.GetChat().EstablishConnection(ServerTextBox.Text);

                // Then we try to authenticate the user with the username he/she gave:
                var authentication = Program.MainMenu.GetChat().AuthenticateUser(UsernameTextBox.Text);
                await authentication;
                if (authentication.Result)
                {
                    // We initialize the chat to activate broadcasting from the server:
                    await Program.MainMenu.GetChat().InitializeChat(loginForm);
                    // Finally we move from the login page to the main menu:
                    Program.FormManager.CurrentForm = Program.MainMenu;
                }
                else
                {
                    UsernameTextBox.Clear();
                    throw new LoginException(@"Ce nom d'utilisateur est déjà pris. Veuillez en choisir un autre.");
                }
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
