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
            bool isValidIp = System.Net.IPAddress.TryParse(ServerTextBox.Text, out IPAddress ipAddress) || "localhost".Equals(ServerTextBox.Text);


            try
            {
                if (!isValidIp)
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


                Program.client = new HttpClient();
                Program.client.DefaultRequestHeaders.Accept.Clear();
                Program.client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                Program.client.BaseAddress = new Uri("http://" + ServerTextBox.Text + ":63056");

                //Before we actually connect, we check with the server if the login is taken]\
                HttpStatusCode response = await LoginClient.PostLoginAsync(loginForm);

                //We initiate the chat connection
                if (response.GetHashCode() == 200)
                {
                    Program.MainMenu.InitializeChat(loginForm, ServerTextBox.Text);

                    Program.FormManager.CurrentForm = Program.MainMenu;


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
