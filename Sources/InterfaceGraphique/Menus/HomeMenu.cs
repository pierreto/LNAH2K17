﻿using System;
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
    public partial class HomeMenu : Form
    {
        public HomeMenu()
        {
            InitializeComponent();
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

        private void InitializeComponent()
        {
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.homeView1 = new InterfaceGraphique.Controls.WPF.Home.HomeView();
            this.SuspendLayout();
            // 
            // elementHost1
            // 
            this.elementHost1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementHost1.Location = new System.Drawing.Point(0, 0);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(284, 261);
            this.elementHost1.TabIndex = 0;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = this.homeView1;
            // 
            // HomeMenu
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.elementHost1);
            this.Name = "HomeMenu";
            this.ResumeLayout(false);

        }
    }
}
