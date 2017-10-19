using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InterfaceGraphique.Menus
{
    public partial class ConnectServerMenu : Form
    {

        private HubManager hubManager;
        private readonly string LOCALHOST = "localhost";
        public ConnectServerMenu()
        {
            InitializeComponent();
            InitializeEvents();
            this.hubManager = new HubManager();
            this.ipAddressErrorLabel.Text = "";
        }

        private void InitializeEvents()
        {
            this.ConnectServerButton.Click += async (sender, e) =>
            {
                await ConnectToServer();
            };

            this.IpAddressInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(CheckEnterKeyPress);
        }

        private async Task ConnectToServer()
        {
            this.ConnectServerButton.Enabled = false;

            try
            {
                ValidateIpAddress();
                await hubManager.EstablishConnection(IpAddressInput.Text);
                Program.FormManager.CurrentForm = Program.MainMenu;
            }
            catch (LoginException e)
            {
                Console.WriteLine(e);
                this.NotifyError(this.IpAddressInput);
                this.ipAddressErrorLabel.Text = "Adresse IP invalide";
            }
            catch (System.Exception exception)
            {
                //Server Exceptions...
                Console.WriteLine(exception);
                this.NotifyError(this.IpAddressInput);
                this.ipAddressErrorLabel.Text = "Adresse non rejoinable";
            }
            finally
            {
                this.ConnectServerButton.Enabled = true;
            }
        }

        private void ValidateIpAddress()
        {
            if (!ValidateIP(IpAddressInput.Text) && !LOCALHOST.Equals(IpAddressInput.Text))
            {
                throw new LoginException(@"Le format de l'adresse IP n'est pas valide.");
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

        ////////////////////////////////////////////////////////////////////////
        ///
        /// Fonction vide appelée sur toutes les forms de facon 
        /// constante sans se soucier du type
        /// 
        ///	@param[in]  tempsInterAffichage : Temps entre chaque affichage
        /// @return     Void
        ///
        ////////////////////////////////////////////////////////////////////////
        public void MettreAJour(double tempsInterAffichage)
        {

        }

        ////////////////////////////////////////////////////////////////////////
        ///
        /// Met à jour la taille de la fenetre selon celle de la fenêtre parent
        /// 
        ///	@param[in]  sender : Objet qui a causé l'évènement
        /// @param[in]  e : Arguments de l'évènement
        /// @return     Void
        /// 
        ////////////////////////////////////////////////////////////////////////
        private void WindowSizeChanged(object sender, EventArgs e)
        {
            this.Size = new Size(Program.FormManager.ClientSize.Width, Program.FormManager.ClientSize.Height);
        }

        ////////////////////////////////////////////////////////////////////////
        ///
        /// Ajoute le panneau openGL à la Form en cours. Les controles sont
        /// modifiés afin d'ajouter les éléments visuels nécessaires et les 
        /// events sur le panel sont ajoutés.
        ///
        /// @return Void
        ///
        ////////////////////////////////////////////////////////////////////////
        public void InitializeOpenGlPanel()
        {
            Program.FormManager.SizeChanged += new EventHandler(WindowSizeChanged);
        }

        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction désinscrits les events de la form courante sur le 
        /// panneau openGL.
        ///
        /// @return Void 
        ///
        ////////////////////////////////////////////////////////////////////////
        public void UnsuscribeEventHandlers()
        {
            Program.FormManager.SizeChanged -= new EventHandler(WindowSizeChanged);
        }

        private async void CheckEnterKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            ClearTextBox(this.IpAddressInput);
            this.ipAddressErrorLabel.Text = "";
            if (e.KeyChar == (char)Keys.Return)
            {
                await ConnectToServer();
                e.Handled = true;
            }

        }

        private void NotifyError(TextBox textBox)
        {
            textBox.Text = "";
            textBox.BackColor = ColorTranslator.FromHtml("#F2ACAC");
        }

        private void ClearTextBox(TextBox textBox)
        {
            textBox.BackColor = Color.White;
        }
    }
}
