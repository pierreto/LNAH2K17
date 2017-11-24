using InterfaceGraphique.CommunicationInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Practices.Unity;
using InterfaceGraphique.Controls.WPF.Chat;
using InterfaceGraphique.Controls.WPF.Friends;
using InterfaceGraphique.Managers;

namespace InterfaceGraphique {

    ///////////////////////////////////////////////////////////////////////////
    /// @class FormManager
    /// @brief Fenêtre gestionnaire de toutes les forms c# (parent)
    /// @author Julien Charbonneau
    /// @date 2016-09-13
    ///////////////////////////////////////////////////////////////////////////
    public partial class FormManager : Form {
        private int friendHeight;
        private readonly int COLLAPSED_CHAT_HEIGHT = 40;
        private int chatHeight;

        public dynamic CurrentForm {
            get { return currentForm; }
            set {
                if (currentForm != null) {
                    this.Controls.Remove(currentForm);
                    currentForm.UnsuscribeEventHandlers();
                }

                currentForm = value;
                currentForm.TopLevel = false;
                currentForm.FormBorderStyle = FormBorderStyle.None;
                currentForm.Location = new Point(0, 0);
                currentForm.Size = new Size(this.ClientSize.Width, this.ClientSize.Height);
                currentForm.Visible = true;
         
                currentForm.InitializeOpenGlPanel();
                this.Controls.Add(currentForm);
                currentForm.Focus();


                if (User.Instance.IsConnected)
                {
                    ShowCompletely();
                }
                else
                {
                    HideCompletely();
                }
            }
        }

   

        private dynamic currentForm;


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Constructeur de la classe FormManager
        ///
        ////////////////////////////////////////////////////////////////////////
        public FormManager() {
            InitializeComponent();
            this.elementHost2.Child = Program.unityContainer.Resolve<FriendContentControl>();
            this.friendHeight = this.elementHost2.Height;

            elementHost1.Child = Program.unityContainer.Resolve<TestChatView>();
            chatHeight = elementHost1.Height;

            this.gameRequestPopup.Hide();

            InitializeScreenSize();
            InitializeOpenGLPanel();
            InitializeEvents();

            elementHost1.Size = new Size(this.ClientSize.Width * 2 / 3 + 1, COLLAPSED_CHAT_HEIGHT);
            elementHost1.Location = new Point(0, this.ClientSize.Height - chatHeight);
            elementHost2.Size = new Size(this.ClientSize.Width * 1 / 3, COLLAPSED_CHAT_HEIGHT);
            elementHost2.Location = new Point(this.ClientSize.Width - elementHost2.Width,  this.ClientSize.Height - friendHeight);
        }

        ////////////////////////////////////////////////////////////////////////
        ///
        /// Fonction permettant d'initialiser les évènements qu'on s'abonne
        /// 
        /// @return     Void
        ///
        ////////////////////////////////////////////////////////////////////////
        public void InitializeEvents()
        {
            this.buttonAccept.Click += (sender, e) => OnAcceptGameRequest();
            this.buttonRefus.Click += (sender, e) => OnDeclineGameRequest();
        }

        private async void OnDeclineGameRequest()
        {
            this.gameRequestPopup.Hide();
            await Program.unityContainer.Resolve<GameRequestManager>().DeclineGameRequest();
        }

        private async void OnAcceptGameRequest()
        {
            this.gameRequestPopup.Hide();
            await Program.unityContainer.Resolve<GameRequestManager>().AcceptGameRequest();
        }

        public void ShowGameRequestPopup()
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                this.gameRequestPopup.Show();
            }));
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Fonction permettant de mettre à jour l'affichage openGL selon un 
        /// temps interaffichage désiré
        /// 
        ///	@param[in]  tempsInterAffichage : Temps entre chaque affichage
        /// @return     Void
        ///
        ////////////////////////////////////////////////////////////////////////
        public void MettreAJour(double tempsInterAffichage) {
            CurrentForm.MettreAJour(tempsInterAffichage);
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Fonction permettant d'initialiser les paramètres de taille par 
        /// défaut de la fenêtre à l'ouverture du logiciel.
        ///
        /// @return FormManager
        ///
        ////////////////////////////////////////////////////////////////////////
        public void InitializeScreenSize() {
            int screenResolutionX = Screen.PrimaryScreen.Bounds.Width;
            int screenResolutionY = Screen.PrimaryScreen.Bounds.Height;

            this.MaximumSize = new Size(screenResolutionX, screenResolutionY);
            this.MinimumSize = new Size(800, 600);
            this.Size = new Size((int)(screenResolutionX * 0.75), (int)(screenResolutionY * 0.75));
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Initialise le panneau openGL avec les bonnes propriétés et
        /// initialise le contexte openGL.
        ///
        /// @return FormManager
        ///
        ////////////////////////////////////////////////////////////////////////
        private void InitializeOpenGLPanel() {
            this.SizeChanged += new EventHandler(WindowSizeChanged);

            Program.OpenGLPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            Program.OpenGLPanel.Location = new System.Drawing.Point(0, 0);
            Program.OpenGLPanel.Name = "OpenGL";

            this.DoubleBuffered = false;
            FonctionsNatives.initialiserOpenGL(Program.OpenGLPanel.Handle);
            FonctionsNatives.dessinerOpenGL();
        }

        private void WindowSizeChanged(object sender, EventArgs e)
        {
            this.elementHost1.Size = new Size(this.ClientSize.Width * 3 / 4 + 1, elementHost1.Size.Height);
            this.elementHost2.Size = new Size(this.ClientSize.Width * 1 / 4, elementHost2.Size.Height);
            elementHost2.Location = new Point(this.ClientSize.Width - elementHost2.Width, elementHost2.Location.Y);
        }

        public void MinimizeFriendList()
        {
            HideFriendList();
            elementHost2.Size = new Size(elementHost2.Width, COLLAPSED_CHAT_HEIGHT);
            elementHost2.Location = new Point(elementHost2.Location.X, elementHost2.Location.Y + friendHeight - COLLAPSED_CHAT_HEIGHT);
            ShowFriendList();
        }

        public void MaximizeFriendList()
        {
            HideFriendList();
            elementHost2.Size = new Size(elementHost2.Width, friendHeight);
            elementHost2.Location = new Point(elementHost2.Location.X, elementHost2.Location.Y - friendHeight + COLLAPSED_CHAT_HEIGHT);
            ShowFriendList();
        }

        public void HideFriendList()
        {
            this.elementHost2.Hide();
        }
        public void ShowFriendList()
        {
            this.elementHost2.Show();

        }
        public void HideCompletely()
        {
            HideFriendList();
            HideChat();

            //elementHost1.Size = new Size(0, 0);
        }

        private void ShowCompletely()
        {
            ShowFriendList();
            ShowChat();
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
            elementHost1.Size = new Size(elementHost1.Width, chatHeight);
            elementHost1.Location = new Point(elementHost1.Location.X, elementHost1.Location.Y - chatHeight + COLLAPSED_CHAT_HEIGHT);
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
    }
}
