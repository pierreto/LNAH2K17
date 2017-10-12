using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InterfaceGraphique {

    ///////////////////////////////////////////////////////////////////////////
    /// @class FormManager
    /// @brief Fenêtre gestionnaire de toutes les forms c# (parent)
    /// @author Julien Charbonneau
    /// @date 2016-09-13
    ///////////////////////////////////////////////////////////////////////////
    public partial class FormManager : Form {

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
            InitializeScreenSize();
            InitializeOpenGLPanel();
            InitializeEvents();
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
            this.FormClosing += (sender, e) => Program.Login.Logout();
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
            Program.OpenGLPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            Program.OpenGLPanel.Location = new System.Drawing.Point(0, 0);
            Program.OpenGLPanel.Name = "OpenGL";

            this.DoubleBuffered = false;
            FonctionsNatives.initialiserOpenGL(Program.OpenGLPanel.Handle);
            FonctionsNatives.dessinerOpenGL();
        }
    }
}
