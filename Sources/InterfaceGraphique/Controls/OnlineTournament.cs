using InterfaceGraphique.Controls.WPF.Chat;
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

namespace InterfaceGraphique.Controls
{
    public partial class OnlineTournament : Form
    {

        private int chatHeight;
        private Point chatLocation;
        private readonly int COLLAPSED_CHAT_HEIGHT = 40;

        public OnlineTournament()
        {
            InitializeComponent();
            chatHeight = elementHost2.Height;
            chatLocation = elementHost2.Location;
            //Make sure it is visible first so when we toggle with minimize it gets minimazed instead of maximized
            Program.unityContainer.Resolve<ChatViewModel>().Collapsed = System.Windows.Visibility.Visible;
            Program.unityContainer.Resolve<ChatViewModel>().Minimize();
            MinimizeChat();
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

        public void MinimizeChat()
        {
            HideChat();
            elementHost2.Size = new Size(elementHost2.Width, COLLAPSED_CHAT_HEIGHT);
            elementHost2.Location = new Point(elementHost2.Location.X, elementHost2.Location.Y + chatHeight - COLLAPSED_CHAT_HEIGHT);
            ShowChat();
        }

        public void MaximizeChat()
        {
            HideChat();
            elementHost2.Location = chatLocation;
            elementHost2.Size = new Size(elementHost2.Width, chatHeight);
            ShowChat();
        }
        public void HideChat()
        {
            this.elementHost2.Hide();
        }

        public void ShowChat()
        {
            this.elementHost2.Show();
        }

        public void HideCompletely()
        {
            HideChat();
            elementHost2.Size = new Size(0, 0);
        }
    }
}
