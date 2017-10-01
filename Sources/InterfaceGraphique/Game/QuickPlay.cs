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
    /// @class QuickPlay
    /// @brief UI du mode partie rapide
    /// @author Julien Charbonneau
    /// @date 2016-09-13
    ///////////////////////////////////////////////////////////////////////////
    public partial class QuickPlay : Form {

        ////////////////////////////////////////////////////////////////////////
        ///
        /// Constructeur de la classe QuickPlay
        ///
        ////////////////////////////////////////////////////////////////////////
        public QuickPlay() {
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
            Program.OpenGLPanel.Controls.Add(this.Panel_EndBack);
            Program.OpenGLPanel.Controls.Add(this.MenuStrip_MenuBar);
            this.Controls.Add(Program.OpenGLPanel);

            Program.FormManager.SizeChanged += new EventHandler(WindowSizeChanged);
            Program.FormManager.LocationChanged += new EventHandler(DessinerOpenGL);
            Program.OpenGLPanel.MouseMove += new MouseEventHandler(MouseMoved);
            Program.OpenGLPanel.MouseWheel += new MouseEventHandler(MouseWheelScrolled);

            this.Panel_EndBack.Location = new Point((Program.OpenGLPanel.Width - this.Panel_EndBack.Width) / 2, (Program.OpenGLPanel.Height - this.Panel_EndBack.Height) / 2);

            this.MenuStrip_MenuBar.Visible = false;
            this.Panel_EndBack.Visible = false;
            this.MenuStrip_MenuBar.Renderer = new Renderer_MenuBar();

            keyUp = Program.ConfigurationMenu.MoveUpKey;
            keyDown = Program.ConfigurationMenu.MoveDownKey;
            keyLeft = Program.ConfigurationMenu.MoveLeftKey;
            keyRight = Program.ConfigurationMenu.MoveRightKey;
            neededGoalsToWin = Program.ConfigurationMenu.NeededGoalsToWin;

            LoadMap();
            ToggleOrbit(true);

            FonctionsNatives.redimensionnerFenetre(this.Size.Width, this.Size.Height);
            FonctionsNatives.changeGridVisibility(false);
            FonctionsNatives.gererRondelleMaillets(true);
            FonctionsNatives.changerModeleEtat((int)MODELE_ETAT.JEU);
            FonctionsNatives.toggleControlPointsVisibility(false);
            FonctionsNatives.toggleTestMode(false);
            FonctionsNatives.setLights(0, true);
            FonctionsNatives.setLights(1, true);
            FonctionsNatives.setLights(2, true);
            FonctionsNatives.resetGame();
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction initialise les événements sur la form
        ///
        /// @return Void 
        ///
        ////////////////////////////////////////////////////////////////////////
        private void InitializeEvents() {
            this.MenuItem_MainMenu.Click += (sender, e) => { ResetDefaultTable(); Program.FormManager.CurrentForm = Program.MainMenu; };
            this.MenuItem_Help.Click += (sender, e) => { EditorHelp form = new EditorHelp(); form.ShowQuickPlayHelpText(); form.ShowDialog(); };
            this.MenuItem_OrbitView.Click += (sender, e) => ToggleOrbit(true);
            this.MenuItem_OrthoView.Click += (sender, e) => ToggleOrbit(false);
            this.Button_MainMenu.Click += (sender, e) => { ResetDefaultTable(); Program.FormManager.CurrentForm = Program.MainMenu; };
            this.Button_PlayAgain.Click += (sender, e) => Program.FormManager.CurrentForm = Program.QuickPlay;
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
        /// @return     Aucune 
        ///
        ////////////////////////////////////////////////////////////////////////
        public void MettreAJour(double tempsInterAffichage) {
            try {
                this.Invoke((MethodInvoker)delegate {
                    FonctionsNatives.moveMaillet();
                    FonctionsNatives.animer(tempsInterAffichage);
                    FonctionsNatives.dessinerOpenGL();
                    if (FonctionsNatives.isGameOver(neededGoalsToWin) == 1)
                        EndGame();
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
        /// Cette fonction permet de remettre la table à zéro (points de contrôle
        /// en position initiale et enfants détruits).
        ///
        /// @return Void 
        ///
        ///////////////////////////////////////////////////////////////////////
        private void ResetDefaultTable() {
            Program.GeneralProperties.ResetProperties();
            FonctionsNatives.resetNodeTree();
            FonctionsNatives.resetCameraPosition();
            FonctionsNatives.redimensionnerFenetre(this.Size.Width, this.Size.Height);
            FonctionsNatives.playMusic(false);
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction permet de charger la carte active
        ///
        /// @return Void 
        ///
        ///////////////////////////////////////////////////////////////////////
        private void LoadMap() {
            ResetDefaultTable();
            FonctionsNatives.playMusic(true);

            if (mapFilePath != null) {
                StringBuilder filePath = new StringBuilder(mapFilePath.Length);
                filePath.Append(mapFilePath);
                float[] coefficients = new float[3];
                FonctionsNatives.ouvrir(filePath, coefficients);
                Program.GeneralProperties.SetCoefficientValues(coefficients);
            }
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
        private void MouseMoved(object sender, MouseEventArgs e){
            FonctionsNatives.mouseMove(e.Location.X, e.Location.Y);
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
        /// @return     Aucune 
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
        /// Cette fonction s'occupe de gérer la fin de partie.
        ///
        /// @return Void 
        ///
        ////////////////////////////////////////////////////////////////////////
        private void EndGame() {
            int[] score = new int[2];
            FonctionsNatives.getGameScore(score);

            if (isTournementMode) {      
                Program.TournementTree.RoundScore = score;
                Program.FormManager.CurrentForm = Program.TournementTree;
            }
            else {
                this.Panel_EndBack.Visible = true;
                this.Label_Score.Text = score[0] + " - " + score[1];        
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
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            switch(keyData) {
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
                    if (!this.Panel_EndBack.Visible && FonctionsNatives.isGameStarted()) {
                        this.MenuStrip_MenuBar.Visible = !this.MenuStrip_MenuBar.Visible;
                        FonctionsNatives.escape();
                    }
                    return true;

                case Keys.Space:
                    Program.FormManager.CurrentForm = Program.QuickPlay;
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
                    Program.FormManager.CurrentForm = Program.MainMenu;
                    return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
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

        private int neededGoalsToWin = 2;
        private bool isTournementMode;

        private string mapFilePath;


        // Accessors
        public bool IsTournementMode { set { isTournementMode = value; } }
        public string MapFilePath { set { mapFilePath = value; } }
    }
}
