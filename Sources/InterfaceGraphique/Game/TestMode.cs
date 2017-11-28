using InterfaceGraphique.Controls.WPF.MainMenu;
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

namespace InterfaceGraphique {

    ///////////////////////////////////////////////////////////////////////////
    /// @class TestMode
    /// @brief UI du mode partie test
    /// @author Julien Charbonneau
    /// @date 2016-09-13
    ///////////////////////////////////////////////////////////////////////////
    public partial class TestMode : Form {

        ////////////////////////////////////////////////////////////////////////
        ///
        /// Constructeur de la classe TestMode
        ///
        ////////////////////////////////////////////////////////////////////////
        public TestMode() {
            InitializeComponent();
            InitializeEvents();
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
        public void InitializeOpenGlPanel() {
            Program.OpenGLPanel.Controls.Clear();
            Program.OpenGLPanel.Controls.Add(this.MenuStrip_MenuBar);
            this.Controls.Add(Program.OpenGLPanel);

            Program.FormManager.SizeChanged += new EventHandler(WindowSizeChanged);
            Program.FormManager.LocationChanged += new EventHandler(DessinerOpenGL);
            Program.OpenGLPanel.MouseMove += new MouseEventHandler(MouseMoved);
            Program.OpenGLPanel.MouseWheel += new MouseEventHandler(MouseWheelScrolled);

            this.MenuStrip_MenuBar.Visible = false;
            this.MenuStrip_MenuBar.Renderer = new Renderer_MenuBar();

            keyUp = Program.ConfigurationMenu.MoveUpKey;
            keyDown = Program.ConfigurationMenu.MoveDownKey;
            keyLeft = Program.ConfigurationMenu.MoveLeftKey;
            keyRight = Program.ConfigurationMenu.MoveRightKey;

            ToggleOrbit(true);

            FonctionsNatives.resetCameraPosition();
            FonctionsNatives.redimensionnerFenetre(this.Size.Width, this.Size.Height);
            FonctionsNatives.changeGridVisibility(false);
            FonctionsNatives.gererRondelleMaillets(false);
            FonctionsNatives.gererRondelleMaillets(true);
            FonctionsNatives.changerModeleEtat((int)MODELE_ETAT.JEU);
            FonctionsNatives.toggleControlPointsVisibility(false);
            FonctionsNatives.toggleTestMode(true);
            FonctionsNatives.setLights(0, true);
            FonctionsNatives.setLights(1, true);
            FonctionsNatives.setLights(2, true);
            FonctionsNatives.resetGame();

            PlayerProfile selectedProfile = Program.ConfigurationMenu.GetProfile(null);
            FonctionsNatives.aiActiveProfile(selectedProfile.Speed, selectedProfile.Passivity);

            OpponentType opponentType;
            if (Program.ConfigurationMenu.IsPlayer2Virtual)
            {
                opponentType = OpponentType.VIRTUAL_PLAYER;
            }
            else
            {
                opponentType = OpponentType.LOCAL_PLAYER;
            }

            FonctionsNatives.setCurrentOpponentType((int) opponentType);
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction initialise les événements
        ///
        /// @return Void 
        ///
        ////////////////////////////////////////////////////////////////////////
        private void InitializeEvents() {
            this.MenuItem_EditorMode.Click += (sender, e) => Program.FormManager.CurrentForm = Program.Editeur;
            this.MenuItem_MainMenu.Click += (sender, e) => { Program.FormManager.CurrentForm = Program.HomeMenu; Program.HomeMenu.ChangeViewTo(Program.unityContainer.Resolve<MainMenuViewModel>()); };
            this.MenuItem_OrbitView.Click += (sender, e) => ToggleOrbit(true);
            this.MenuItem_OrthoView.Click += (sender, e) => ToggleOrbit(false);
            this.MenuItem_Help.Click += (sender, e) => { EditorHelp form = new EditorHelp(); form.ShowTestHelpText(); form.ShowDialog(); };
            this.KeyDown += new KeyEventHandler(KeyDownEvent);
            this.KeyUp += new KeyEventHandler(KeyUpEvent);
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction désinscrits les events de la form courante sur le 
        /// panneau openGL.
        ///
        /// @return Void 
        ///
        ////////////////////////////////////////////////////////////////////////
        public void UnsuscribeEventHandlers() {
            Program.FormManager.SizeChanged -= new EventHandler(WindowSizeChanged);
            Program.FormManager.LocationChanged -= new EventHandler(DessinerOpenGL);
            Program.OpenGLPanel.MouseMove -= new MouseEventHandler(MouseMoved);
            Program.OpenGLPanel.MouseWheel -= new MouseEventHandler(MouseWheelScrolled);
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction fait la mise à jour de l'interface
        /// 
        /// @param[in]  tempsInterAffichage : Temps entre deux affichages
        /// @return     Void 
        ///
        ////////////////////////////////////////////////////////////////////////
        public void MettreAJour(double tempsInterAffichage) {
            try {
                this.Invoke((MethodInvoker)delegate {
                    FonctionsNatives.moveMaillet();
                    FonctionsNatives.animer(tempsInterAffichage);
                    FonctionsNatives.dessinerOpenGL();
                });
            }
            catch (Exception) {

            }
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction prend en charge l'événement d'une modification de la
        /// taille de la fenêtre du programme.
        /// 
        /// @param[in]  sender : L'objet qui envoie l'événement (fenêtre)
        /// @param[in]  e      : Propriétés de l'événement
        /// @return     Void 
        ///
        ////////////////////////////////////////////////////////////////////////
        private void WindowSizeChanged(object sender, EventArgs e) {
            this.Size = new Size(Program.FormManager.ClientSize.Width, Program.FormManager.ClientSize.Height);
            FonctionsNatives.redimensionnerFenetre(this.Size.Width, this.Size.Height);
            FonctionsNatives.dessinerOpenGL();
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction actualise l'affichage openGL
        /// 
        /// @param[in]  sender : L'objet qui envoie l'événement
        /// @param[in]  e      : Propriétés de l'événement
        /// @return     Void 
        ///
        ////////////////////////////////////////////////////////////////////////
        private void DessinerOpenGL(object sender, EventArgs e) {
            FonctionsNatives.dessinerOpenGL();
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction reinitialise la table et la caméra aux valeurs par
        /// défaut.
        ///
        /// @return Void 
        ///
        ////////////////////////////////////////////////////////////////////////
        private void ResetDefaultTable() {
            Program.GeneralProperties.ResetProperties();
            FonctionsNatives.resetNodeTree();
            FonctionsNatives.resetCameraPosition();
            FonctionsNatives.redimensionnerFenetre(this.Size.Width, this.Size.Height);
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction suit le mouvement de la souris.
        ///
        /// @param[in]  sender : L'objet qui envoie l'événement
        /// @param[in]  e      : Propriétés de l'événement
        /// @return     Void 
        ///
        ////////////////////////////////////////////////////////////////////////
        private void MouseMoved(object sender, MouseEventArgs e) {
            FonctionsNatives.playerMouseMove(e.Location.X, e.Location.Y);
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction gère l'enfoncement des touches de déplacement du
        /// joueur 2 et assigne une vitesse au maillet.
        ///
        /// @param[in]  sender : L'objet qui envoie l'événement
        /// @param[in]  e      : Propriétés de l'événement
        /// @return     Void 
        ///
        ////////////////////////////////////////////////////////////////////////
        private void KeyDownEvent(object sender, KeyEventArgs e) {
            if (e.KeyCode == keyUp && !moveUpKeyDown) {
                moveUpKeyDown = true;
                moveDownKeyDown = false;
                FonctionsNatives.setSpeedYMaillet(GlobalVariables.speedMaillet);
            }
            if (e.KeyCode == keyLeft && !moveLeftKeyDown) {
                moveLeftKeyDown = true;
                moveRightKeyDown = false;
                FonctionsNatives.setSpeedXMaillet(-GlobalVariables.speedMaillet);
            }
            if (e.KeyCode == keyDown && !moveDownKeyDown) {
                moveDownKeyDown = true;
                moveUpKeyDown = false;
                FonctionsNatives.setSpeedYMaillet(-GlobalVariables.speedMaillet);
            }
            if (e.KeyCode == keyRight && !moveRightKeyDown) {
                moveRightKeyDown = true;
                moveLeftKeyDown = false;
                FonctionsNatives.setSpeedXMaillet(GlobalVariables.speedMaillet);
            }
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction gère le relachement des touches de déplacement du
        /// joueur 2 et retire la vitesse au maillet.
        ///
        /// @param[in]  sender : L'objet qui envoie l'événement
        /// @param[in]  e      : Propriétés de l'événement
        /// @return     Void 
        ///
        ////////////////////////////////////////////////////////////////////////
        private void KeyUpEvent(object sender, KeyEventArgs e) {
            if (e.KeyCode == keyUp && moveUpKeyDown) {
                moveUpKeyDown = false;
                FonctionsNatives.setSpeedYMaillet(0);
            }
            if (e.KeyCode == keyLeft && moveLeftKeyDown) {
                moveLeftKeyDown = false;
                FonctionsNatives.setSpeedXMaillet(0);
            }
            if (e.KeyCode == keyDown && moveDownKeyDown) {
                moveDownKeyDown = false;
                FonctionsNatives.setSpeedYMaillet(0);
            }
            if (e.KeyCode == keyRight && moveRightKeyDown) {
                moveRightKeyDown = false;
                FonctionsNatives.setSpeedXMaillet(0);
            }
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction prend en charge l'événement de la roulette de la 
        /// souris (dans les deux directions).
        /// 
        /// @param[in]  sender : L'objet qui envoie l'événement (souris)
        /// @param[in]  e      : Propriétés de l'événement
        /// @return     Void 
        ///
        ////////////////////////////////////////////////////////////////////////
        private void MouseWheelScrolled(object sender, MouseEventArgs e) {
            if (e.Delta > 0) {
                for (int i = 0; i < 3; i++) {
                    FonctionsNatives.zoomIn();
                }
            }

            if (e.Delta < 0) {
                for (int i = 0; i < 3; i++) {
                    FonctionsNatives.zoomOut();
                }
            }
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction permet changer en le mode orbit et le mode 
        /// perspective.
        /// 
        /// @param[in]  isOrbit : Etat d'activation du mode orbit
        /// @return     Void 
        ///
        ////////////////////////////////////////////////////////////////////////
        private void ToggleOrbit(bool isOrbit) {
            FonctionsNatives.toggleOrbit(isOrbit);
            this.MenuItem_OrbitView.Enabled = !isOrbit;
            this.MenuItem_OrthoView.Enabled = isOrbit;
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction gère la touche d'espacement et le escape afin de 
        /// faire suivre avec le bon traitement de la touche appuyée.
        /// 
        /// @param[in]  msg          : Message de l'event
        /// @param[in]  keyData      : Touche appuyée
        /// @return     Vrai si la touche est gérée 
        ///
        ////////////////////////////////////////////////////////////////////////
        /*protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            switch (keyData) {
                case Keys.T:
                    Program.FormManager.CurrentForm = Program.Editeur;
                    return true;

                case Keys.J:
                    FonctionsNatives.toggleLights(0);
                    return true;

                case Keys.K:
                    FonctionsNatives.toggleLights(1);
                    return true;

                case Keys.L:
                    FonctionsNatives.toggleLights(2);
                    return true;

                case Keys.Escape:
                    this.MenuStrip_MenuBar.Visible = !this.MenuStrip_MenuBar.Visible;
                    FonctionsNatives.escape();
                    return true;

                case Keys.Space:
                    Program.FormManager.CurrentForm = Program.TestMode;
                    return true;

                case Keys.Up:
                    FonctionsNatives.fleches(0, GlobalVariables.deplacementVue);
                    return true;

                case Keys.Down:
                    FonctionsNatives.fleches(0, -GlobalVariables.deplacementVue);
                    return true;

                case Keys.Left:
                    FonctionsNatives.fleches(-GlobalVariables.deplacementVue, 0);
                    return true;

                case Keys.Right:
                    FonctionsNatives.fleches(GlobalVariables.deplacementVue, 0);
                    return true;

                case Keys.Oemplus:
                    FonctionsNatives.zoomIn();
                    return true;

                case Keys.OemMinus:
                    FonctionsNatives.zoomOut();
                    return true;

                case Keys.D1:
                    ToggleOrbit(false);
                    return true;

                case Keys.D2:
                    ToggleOrbit(true);
                    return true;

                case (Keys.Q | Keys.Control):
                    ResetDefaultTable();
                    Program.FormManager.CurrentForm = Program.HomeMenu;
                    Program.HomeMenu.ChangeViewTo(Program.unityContainer.Resolve<MainMenuViewModel>());
                    return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }*/
        public  bool ProcessCmdKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.T:
                    Program.FormManager.CurrentForm = Program.Editeur;
                    return true;

                case Keys.J:
                    FonctionsNatives.toggleLights(0);
                    return true;

                case Keys.K:
                    FonctionsNatives.toggleLights(1);
                    return true;

                case Keys.L:
                    FonctionsNatives.toggleLights(2);
                    return true;

                case Keys.Escape:
                    this.MenuStrip_MenuBar.Visible = !this.MenuStrip_MenuBar.Visible;
                    FonctionsNatives.escape();
                    return true;

                case Keys.Space:
                    Program.FormManager.CurrentForm = Program.TestMode;
                    return true;

                case Keys.Up:
                    FonctionsNatives.fleches(0, GlobalVariables.deplacementVue);
                    return true;

                case Keys.Down:
                    FonctionsNatives.fleches(0, -GlobalVariables.deplacementVue);
                    return true;

                case Keys.Left:
                    FonctionsNatives.fleches(-GlobalVariables.deplacementVue, 0);
                    return true;

                case Keys.Right:
                    FonctionsNatives.fleches(GlobalVariables.deplacementVue, 0);
                    return true;

                case Keys.Oemplus:
                    FonctionsNatives.zoomIn();
                    return true;

                case Keys.OemMinus:
                    FonctionsNatives.zoomOut();
                    return true;

                case Keys.D1:
                    ToggleOrbit(false);
                    return true;

                case Keys.D2:
                    ToggleOrbit(true);
                    return true;

                case (Keys.Q | Keys.Control):
                    ResetDefaultTable();
                    Program.FormManager.CurrentForm = Program.HomeMenu;
                    Program.HomeMenu.ChangeViewTo(Program.unityContainer.Resolve<MainMenuViewModel>());
                    return true;
            }
            return true;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        private bool moveUpKeyDown = false;
        private bool moveDownKeyDown = false;
        private bool moveLeftKeyDown = false;
        private bool moveRightKeyDown = false;

        private Keys keyUp = Keys.W;
        private Keys keyDown = Keys.S;
        private Keys keyLeft = Keys.A;
        private Keys keyRight = Keys.D;
    }
}
