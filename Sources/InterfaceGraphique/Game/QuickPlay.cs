using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InterfaceGraphique.Controls.WPF.Matchmaking;
using InterfaceGraphique.Editor.EditorState;
using InterfaceGraphique.Game.GameState;
using Microsoft.Practices.Unity;
using InterfaceGraphique.Controls.WPF.Tournament;

namespace InterfaceGraphique
{

    ///////////////////////////////////////////////////////////////////////////
    /// @class QuickPlay
    /// @brief UI du mode partie rapide
    /// @author Julien Charbonneau
    /// @date 2016-09-13
    ///////////////////////////////////////////////////////////////////////////
    public partial class QuickPlay : Form
    {

        private AbstractGameState currentGameState;
        ////////////////////////////////////////////////////////////////////////
        ///
        /// Constructeur de la classe QuickPlay
        ///
        ////////////////////////////////////////////////////////////////////////
        public QuickPlay()
        {
            InitializeComponent();

            currentGameState = new OfflineGameState();

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
        public void InitializeOpenGlPanel()
        {
            Program.OpenGLPanel.Controls.Clear();
            Program.OpenGLPanel.Controls.Add(this.Panel_EndBack);
            Program.OpenGLPanel.Controls.Add(this.MenuStrip_MenuBar);
            this.Controls.Add(Program.OpenGLPanel);

            Program.FormManager.SizeChanged += new EventHandler(WindowSizeChanged);
            Program.FormManager.LocationChanged += new EventHandler(DessinerOpenGL);
            Program.OpenGLPanel.MouseMove += new MouseEventHandler(currentGameState.MouseMoved);
            Program.OpenGLPanel.MouseWheel += new MouseEventHandler(MouseWheelScrolled);

            this.Panel_EndBack.Location = new Point((Program.OpenGLPanel.Width - this.Panel_EndBack.Width) / 2, (Program.OpenGLPanel.Height - this.Panel_EndBack.Height) / 2);

            this.MenuStrip_MenuBar.Visible = false;
            this.Panel_EndBack.Visible = false;
            this.Button_PlayAgain.Visible = false;
            this.MenuStrip_MenuBar.Renderer = new Renderer_MenuBar();

            currentGameState.KeyUp = Program.ConfigurationMenu.MoveUpKey;
            currentGameState.KeyDown = Program.ConfigurationMenu.MoveDownKey;
            currentGameState.KeyLeft = Program.ConfigurationMenu.MoveLeftKey;
            currentGameState.KeyRight = Program.ConfigurationMenu.MoveRightKey;
            currentGameState.NeededGoalsToWin = Program.ConfigurationMenu.NeededGoalsToWin;

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
        private void InitializeEvents()
        {
            this.MenuItem_MainMenu.Click += (sender, e) => OnMainMenuClicked(sender, e);
            this.MenuItem_Help.Click += (sender, e) => { EditorHelp form = new EditorHelp(); form.ShowQuickPlayHelpText(); form.ShowDialog(); };
            this.MenuItem_OrbitView.Click += (sender, e) => ToggleOrbit(true);
            this.MenuItem_OrthoView.Click += (sender, e) => ToggleOrbit(false);
            this.Button_MainMenu.Click += (sender, e) => OnMainMenuClicked(sender, e);
            this.Button_PlayAgain.Click += (sender, e) => { ResetDefaultTable(); Program.FormManager.CurrentForm = Program.QuickPlay; };
            this.KeyDown += new KeyEventHandler(currentGameState.KeyDownEvent);
            this.KeyUp += new KeyEventHandler(currentGameState.KeyUpEvent);
        }

        private async Task OnMainMenuClicked(object sender, EventArgs e)
        {
            Program.FormManager.CurrentForm = Program.MainMenu;
            if (currentGameState.IsOnlineTournementMode)
            {
                await Program.unityContainer.Resolve<TournamentViewModel>().WaitingRoomHub.LeaveTournament();
            }
            else
            {
                await Program.unityContainer.Resolve<MatchmakingViewModel>().WaitingRoomHub.LeaveGame();
            }

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
            Program.FormManager.LocationChanged -= new EventHandler(DessinerOpenGL);
            Program.OpenGLPanel.MouseMove -= new MouseEventHandler(currentGameState.MouseMoved);
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
        public void MettreAJour(double tempsInterAffichage)
        {
            try
            {
                this.Invoke((MethodInvoker)delegate
                {
                    currentGameState.MettreAJour(tempsInterAffichage, currentGameState.NeededGoalsToWin);
                });
            }
            catch (Exception)
            {

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
        private void WindowSizeChanged(object sender, EventArgs e)
        {
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
        private void DessinerOpenGL(object sender, EventArgs e)
        {
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
        private void ResetDefaultTable()
        {
            Program.GeneralProperties.ResetProperties();
            FonctionsNatives.resetNodeTree();
            FonctionsNatives.resetCameraPosition();
            FonctionsNatives.redimensionnerFenetre(this.Size.Width, this.Size.Height);
            FonctionsNatives.playMusic(false);

            if (!currentGameState.IsOnlineTournementMode)
            {
                Program.unityContainer.Resolve<MatchmakingViewModel>().SetDefaultValues();
            }
        }


        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction permet de charger la carte active
        ///
        /// @return Void 
        ///
        ///////////////////////////////////////////////////////////////////////
        private void LoadMap()
        {
            ResetDefaultTable();
            FonctionsNatives.playMusic(true);

            if (currentGameState.selectedMap != null) // online mode
            {
                float[] coefficients = new float[3];
                FonctionsNatives.chargerCarte(new StringBuilder(currentGameState.selectedMap.Json), coefficients);
                Program.GeneralProperties.SetCoefficientValues(coefficients);
            }
            else if (currentGameState.MapFilePath != null) // offline mode
            {
                StringBuilder filePath = new StringBuilder(currentGameState.MapFilePath.Length);
                filePath.Append(currentGameState.MapFilePath);
                float[] coefficients = new float[3];
                FonctionsNatives.ouvrir(filePath, coefficients);
                Program.GeneralProperties.SetCoefficientValues(coefficients);
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
        private void MouseWheelScrolled(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    FonctionsNatives.zoomIn();
                }
            }

            if (e.Delta < 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    FonctionsNatives.zoomOut();
                }
            }
        }


        public void EndGame()
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                int[] score = new int[2];
                FonctionsNatives.getGameScore(score);

                if (currentGameState.IsTournementMode)
                {
                    Program.TournementTree.RoundScore = score;
                    Program.FormManager.CurrentForm = Program.TournementTree;
                }
                else
                {
                    this.Panel_EndBack.Visible = true;
                    this.Label_Score.Text = score[0] + " - " + score[1];
                }
            }));

        }
        public Button GetReplayButton()
        {
            return this.Button_PlayAgain;
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
        private void ToggleOrbit(bool isOrbit)
        {
            FonctionsNatives.toggleOrbit(isOrbit);
            this.MenuItem_OrbitView.Enabled = !isOrbit;
            this.MenuItem_OrthoView.Enabled = isOrbit;
        }

        public void ApplyEsc()
        {
            if (!this.Panel_EndBack.Visible && FonctionsNatives.isGameStarted()) {
                this.MenuStrip_MenuBar.Visible = !this.MenuStrip_MenuBar.Visible;
                FonctionsNatives.escape();
            }
        }

        public void ReplacePlayerByAI()
        {
            ApplyEsc();
            FonctionsNatives.setCurrentOpponentType((int)OpponentType.VIRTUAL_PLAYER);
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
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
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
                    ApplyEsc();
                    currentGameState.gameHub?.SendGamePauseOrResume();
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

        public AbstractGameState CurrentGameState
        {
            get => currentGameState;
            set => currentGameState = value;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



    }
}
