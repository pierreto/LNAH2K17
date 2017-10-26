using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Controls.WPF;
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
using InterfaceGraphique.Controls.WPF.Home;
using System.Net.Http;
using InterfaceGraphique.Entities;

namespace InterfaceGraphique.Menus
{
    public partial class HomeMenu: Form
    {
        //Rendre singleton?
        static HttpClient client = new HttpClient();

        public HomeMenu()
        {
            InitializeComponent();
        }

        public void ChangeViewTo(ViewModelBase vmb)
        {
            navigationView1.DataContext = vmb;
        }

        public async Task Logout()
        {
            var response = await client.PostAsJsonAsync("http://" + HubManager.Instance.IpAddress + ":63056/api/logout", User.Instance.UserEntity);
            HubManager.Instance.Logout();
            Program.FormManager.CurrentForm = Program.HomeMenu;
            ChangeViewTo(Program.unityContainer.Resolve<HomeViewModel>());
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
