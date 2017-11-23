using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Practices.Unity;
using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Controls.WPF.Home;
using System.Net.Http;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using InterfaceGraphique.Controls.WPF.Chat;
using InterfaceGraphique.Controls.WPF.Chat.Channel;
using InterfaceGraphique.Controls.WPF.Friends;
using InterfaceGraphique.Controls.WPF.UserProfile;
using InterfaceGraphique.Controls.WPF.Store;
using InterfaceGraphique.Controls.WPF.Tutorial;
using Application = System.Windows.Forms.Application;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace InterfaceGraphique
{

    ///////////////////////////////////////////////////////////////////////////
    /// @class MainMenu
    /// @brief UI du menu principal
    /// @author Julien Charbonneau
    /// @date 2016-09-13
    ///////////////////////////////////////////////////////////////////////////
    public partial class MainMenu : Form
    {
        static HttpClient client = new HttpClient();

        ////////////////////////////////////////////////////////////////////////
        ///
        /// Constructeur de la classe MainMenu
        ///
        ////////////////////////////////////////////////////////////////////////
        public MainMenu()
        {
            InitializeComponent();
            InitializeEvents();
        }

        ////////////////////////////////////////////////////////////////////////
        ///
        /// Initialise les events sur la form courrante
        ///
        /// @return Void
        ///
        ////////////////////////////////////////////////////////////////////////
        private void InitializeEvents()
        {

            this.boutonPartieRapide.Click += (sender, e) => Program.QuickPlayMenu.ShowDialog();

            //this.boutonPartieRapide.Click += (sender, e) => Program.FormManager.CurrentForm = Program.LobbyHost;
            this.boutonTournoi.Click += (sender, e) => Program.FormManager.CurrentForm = Program.TournementMenu;
            this.buttonConfiguration.Click += (sender, e) => Program.ConfigurationMenu.ShowDialog();
            this.TutorielEditeur.Click += async (sender, e) => await ShowTutorialEditor();
            this.TutorielGame.Click += async (sender, e) => await ShowTutorialGame();

            this.buttonEditeur.Click += (sender, e) =>
            {
                Program.Editeur.ResetDefaultTable();
                Program.FormManager.CurrentForm = Program.Editeur;
            };
            this.Button_Credits.Click += (sender, e) => Program.FormManager.CurrentForm = Program.CreditsMenu;
            this.buttonQuitter.Click += (sender, e) => { Program.unityContainer.Resolve<ChatViewModel>().UndockedChat?.Close(); System.Windows.Forms.Application.Exit();  };
            if (User.Instance.IsConnected)
            {
                this.buttonLogout.Click += async (sender, e) => await this.Logout();
                Program.FormManager.FormClosing += async (sender, e) => await Logout();
                Program.FormManager.FormClosing += (sender, e) => Program.unityContainer.Resolve<ChatViewModel>().UndockedChat?.Close();
            }
            if(User.Instance.IsConnected)
            {
                this.profileButton.Click += async (sender, e) => await OnProfileButtonClicked(sender, e);
                this.storeButton.Click += async (sender, e) => await OnStoreButtonClicked(sender, e);
            }
            else
            {
                this.profileButton.Visible = false;
                this.storeButton.Visible = false;
            }
        }

        private async Task ShowTutorialEditor()
        {
            await Program.unityContainer.Resolve<TutorialViewModel>().SwitchToMatchSlides();
            Form fc = Application.OpenForms["TutorialHost"];
            if (fc == null)
            {
                Program.TutorialHost.ShowDialog();
            }
        }
        private async Task ShowTutorialGame()
        {
            await Program.unityContainer.Resolve<TutorialViewModel>().SwitchToEditorSlides();

            Form fc = Application.OpenForms["TutorialHost"];
            if (fc == null)
            {
                Program.TutorialHost.ShowDialog();
            }
        }

        private async Task OnStoreButtonClicked(object sender, EventArgs e)
        {
            Program.FormManager.CurrentForm = Program.StoreMenu;
            await Program.unityContainer.Resolve<StoreViewModel>().Initialize();
        }

        private async Task OnProfileButtonClicked(object sender, EventArgs e)
        {
            Program.FormManager.CurrentForm = Program.UserProfileMenu;
            await Program.unityContainer.Resolve<UserProfileViewModel>().Initialize();
        }

        public async Task Logout()
        {
            var response = await client.PostAsJsonAsync(Program.client.BaseAddress + "api/logout", User.Instance.UserEntity);
            HubManager.Instance.Logout();
            User.Instance.UserEntity = null;
            User.Instance.IsConnected = false;
            //TODO: KILL HUB CONNECTIONS
            Program.FormManager.CurrentForm = Program.HomeMenu;
            Program.InitializeUnityDependencyInjection(); 
            Program.HomeMenu.ChangeViewTo(Program.unityContainer.Resolve<HomeViewModel>());
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
    }
}
