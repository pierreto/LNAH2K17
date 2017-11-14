﻿using System;
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
        private int chatHeight;
        private Point chatLocation;
        private readonly int COLLAPSED_CHAT_HEIGHT = 40;
        ////////////////////////////////////////////////////////////////////////
        ///
        /// Constructeur de la classe MainMenu
        ///
        ////////////////////////////////////////////////////////////////////////
        public MainMenu()
        {
            InitializeComponent();
            elementHost1.Child = Program.unityContainer.Resolve<TestChatView>();
            elementHost2.Child = Program.unityContainer.Resolve<FriendContentControl>();

            InitializeEvents();
            chatHeight = elementHost1.Height;
            chatLocation = elementHost1.Location;
            if (User.Instance.IsConnected)
            {

            } else
            {
                HideCompletely();
            }
            //if (onlineMode)
            //{
            //    this.buttonLogout = new System.Windows.Forms.Button();
            //    this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            //    this.testChatView = new InterfaceGraphique.Controls.WPF.Chat.TestChatView();
            //}
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
                this.profileButton.Click += (sender, e) => OnProfileButtonClicked(sender, e);
                this.storeButton.Click += (sender, e) => OnStoreButtonClicked(sender, e);
            }
            else
            {
                this.profileButton.Visible = false;
                this.storeButton.Visible = false;
            }
        }

        private void OnStoreButtonClicked(object sender, EventArgs e)
        {
            Program.FormManager.CurrentForm = Program.StoreMenu;
            Program.unityContainer.Resolve<StoreViewModel>().Initialize();
        }

        private void OnProfileButtonClicked(object sender, EventArgs e)
        {
            Program.FormManager.CurrentForm = Program.UserProfileMenu;
            Program.unityContainer.Resolve<UserProfileViewModel>().Initialize();
        }

        public async Task Logout()
        {
            var response = await client.PostAsJsonAsync(Program.client.BaseAddress + "api/logout", User.Instance.UserEntity);
            HubManager.Instance.Logout();
            User.Instance.UserEntity = null;
            User.Instance.IsConnected = false;
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

        public void MinimizeChat()
        {
            HideChat();
            elementHost1.Size = new Size(elementHost1.Width, COLLAPSED_CHAT_HEIGHT);
            elementHost1.Location = new Point(elementHost1.Location.X, elementHost1.Location.Y + chatHeight - COLLAPSED_CHAT_HEIGHT);
            ShowChat();
        }

        public void MaximizeChat()
        {
            HideChat();
            elementHost1.Location = chatLocation;
            elementHost1.Size = new Size(elementHost1.Width, chatHeight);
            ShowChat();
        }
        public void HideChat()
        {
            this.elementHost1.Hide();
        }

        public void ShowChat()
        {
            this.elementHost1.Show();
        }

        public void HideCompletely()
        {
            HideChat();
            elementHost1.Size = new Size(0, 0);
        }
    }
}
