using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using InterfaceGraphique.Controls.WPF.Editor;
using InterfaceGraphique.Editor;
using Microsoft.Practices.Unity;

namespace InterfaceGraphique.Controls.WPF
{
    public partial class EditorHost : Form
    {
        private OfflineOrOnlineView offlineOrOnlineView;
        private EditorServerBrowser serverBrowser;
        private EditorModeView modeView;
        private PasswordDialog passwordDialog;
        public EditorHost()
        {
            InitializeComponent();
            serverBrowser = new EditorServerBrowser();
            modeView = new EditorModeView();
            passwordDialog= new PasswordDialog();
            offlineOrOnlineView = new OfflineOrOnlineView();
        }

        public async Task SwitchViewToServerBrowser()
        {
            this.WindowState = FormWindowState.Normal;
            this.Width = 1100;
            this.Height = 800;
            this.elementHost1.Child = serverBrowser;
            await Program.unityContainer.Resolve<EditorViewModel>().InitializeViewModelAsync();
        }
        public void SwitchViewToMapModeView()
        {
            this.WindowState = FormWindowState.Normal;
            this.Width = 640;
            this.Height = 325;
            this.elementHost1.Child = modeView;
            Program.unityContainer.Resolve<CreateMapViewModel>().InitializeViewModel();

        }
        public void SwitchViewToOfflineOrOnlineView()
        {
            this.WindowState = FormWindowState.Normal;
            this.Width = 440;
            this.Height = 180;
            this.elementHost1.Child = offlineOrOnlineView;

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

        public PasswordDialog PasswordDialog
        {
            get => passwordDialog;
            set => passwordDialog = value;
        }

        public void LocalSaveAndCloseThreadSafe()
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                Program.unityContainer.Resolve<MapManager>().ManageSavingLocalMap();

                this.Close();
            }));
        }
    }
}
