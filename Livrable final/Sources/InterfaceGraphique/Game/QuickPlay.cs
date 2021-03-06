﻿using System;
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
using InterfaceGraphique.CommunicationInterface.WaitingRooms;
using InterfaceGraphique.Entities;
using InterfaceGraphique.CommunicationInterface;
using System.IO;
using InterfaceGraphique.Controls.WPF.Tutorial;
using InterfaceGraphique.Controls.WPF.MainMenu;
using InterfaceGraphique.Controls;
using InterfaceGraphique.Controls.WPF.Friends;

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
            this.KeyPreview = true;

            InitializeComponent();
            //Application.AddMessageFilter(new MessageFilter { Main = this });
            currentGameState = new OfflineGameState();
            
            InitializeEvents();

            this.KeyPreview = true;
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

            if(Program.unityContainer.Resolve<GameManager>().CurrentOnlineGame == null)
            {
                currentGameState.NeededGoalsToWin = Program.ConfigurationMenu.NeededGoalsToWin;
            }
            else
            {
                currentGameState.NeededGoalsToWin = 2;
            }

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

            if (User.Instance.IsConnected && this.currentGameState.gameHub != null)
            {
                this.CurrentGameState.gameHub.EndOfGameStatsEvent += OnEndOfGameStats;
            }

            if(User.Instance.IsConnected)
            {
                this.playerName1.Text = User.Instance.UserEntity.Username;
                User.Instance.UserEntity.IsPlaying = true;
                Program.unityContainer.Resolve<FriendListViewModel>().OnPropertyChanged("CanShowPlay");
                if (Program.unityContainer.Resolve<FriendListViewModel>().FriendList != null)
                {
                    foreach (FriendListItemViewModel flivm in Program.unityContainer.Resolve<FriendListViewModel>().FriendList)
                    {
                        flivm.OnPropertyChanged("CanSendPlay");
                    }
                }
            }
            else
            {
                this.playerName1.Text = "Joueur 1";
            }

            Program.QuickPlay.CurrentGameState.GameInitialized = true;

            this.Focus();
           this.BringToFront();
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
            this.KeyPreview = true;

            this.MenuItem_MainMenu.Click += async (sender, e) => await OnMainMenuClicked(sender, e);
            this.MenuItem_Help.Click += (sender, e) => { EditorHelp form = new EditorHelp(); form.ShowQuickPlayHelpText(); form.ShowDialog(); };
            this.MenuItem_OrbitView.Click += (sender, e) => ToggleOrbit(true);
            this.MenuItem_OrthoView.Click += (sender, e) => ToggleOrbit(false);
            this.Button_MainMenu.Click += async (sender, e) => await OnMainMenuClicked(sender, e);
            this.Button_PlayAgain.Click += (sender, e) => { ResetDefaultTable(); Program.FormManager.CurrentForm = Program.QuickPlay; };
            this.Informations_Tutoriel.Click += async (sender, e) =>
            {
                await Program.unityContainer.Resolve<TutorialViewModel>().SwitchToMatchSlides();
                Program.TutorialHost.ShowDialog();
            };
            this.KeyDown += new KeyEventHandler(currentGameState.KeyDownEvent);
            this.KeyUp += new KeyEventHandler(currentGameState.KeyUpEvent);

        }

        public void Restart()
        {
            if (this.currentGameState.gameHub != null)
            {
                this.CurrentGameState.gameHub.EndOfGameStatsEvent -= OnEndOfGameStats;
            }

            var gameManager = Program.unityContainer.Resolve<GameManager>();
            gameManager.CurrentOnlineGame = null;

            currentGameState = new OfflineGameState();
            FonctionsNatives.setCurrentOpponentType((int)OpponentType.LOCAL_PLAYER);
            FonctionsNatives.setOnlineClientType((int)OnlineClientType.OFFLINE_GAME);

            Program.QuickPlay.CurrentGameState.IsOnlineTournementMode = false;
        }

        private void OnEndOfGameStats(PlayerEndOfGameStatsEntity stats)
        {
            if(stats.Id != User.Instance.UserEntity.Id)
            {
                return;
            }

            this.BeginInvoke(new MethodInvoker(delegate
            {
                this.pointsNb.Text = "+" + stats.PointsWon;
                List<PictureBox> pictures = new List<PictureBox>();
                pictures.Add(this.achievement1);
                pictures.Add(this.achievement2);
                pictures.Add(this.achievement3);
                for (int i = 0; i < stats.UnlockedAchievements.Count; i++)
                {
                    if(i < pictures.Count)
                    {
                        pictures[i].Visible = true;
                        pictures[i].ImageLocation = Directory.GetCurrentDirectory() + stats.UnlockedAchievements[i].EnabledImageUrl;
                        pictures[i].SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                }
            }));

            if(this.currentGameState.gameHub != null)
            {
                this.CurrentGameState.gameHub.EndOfGameStatsEvent -= OnEndOfGameStats;
            }
        }

        private async Task OnMainMenuClicked(object sender, EventArgs e)
        {
            Program.FormManager.CurrentForm = Program.HomeMenu;
            Program.HomeMenu.ChangeViewTo(Program.unityContainer.Resolve<MainMenuViewModel>());
            this.currentGameState.gameHasEnded = true;
            
            ResetEndOfGameStats();
            Restart();

            if (!User.Instance.IsConnected)
            {
                return;
            }

            User.Instance.UserEntity.IsPlaying = false;
            Program.unityContainer.Resolve<FriendListViewModel>().OnPropertyChanged("CanShowPlay");
            if (Program.unityContainer.Resolve<FriendListViewModel>().FriendList != null)
            {
                foreach (FriendListItemViewModel flivm in Program.unityContainer.Resolve<FriendListViewModel>().FriendList)
                {
                    flivm.OnPropertyChanged("CanSendPlay");
                }
            }
            if (currentGameState.IsOnlineTournementMode)
            {
                await Program.unityContainer.Resolve<TournamentViewModel>().WaitingRoomHub.LeaveTournament();
            }
            else
            {
                await Program.unityContainer.Resolve<MatchmakingViewModel>().WaitingRoomHub.LeaveGame();
            }

        }

        private void ResetEndOfGameStats()
        {
            this.achievement1.ImageLocation = "";
            this.achievement2.ImageLocation = "";
            this.achievement3.ImageLocation = "";
            this.pointsNb.Text = "+0";
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

            // Invisible hors ligne:
            this.Button_ShareOnFacebook.Visible = false;

            currentGameState.IsOnline = false;
            currentGameState.IsOnlineTournementMode = false;

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

            if (currentGameState.selectedMap != null && Program.unityContainer.Resolve<GameManager>().CurrentOnlineGame != null) // online mode
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


        public void EndGame(bool isOnlineGame)
        {
            Program.QuickPlay.CurrentGameState.GameInitialized = false;
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
                    if (User.Instance.IsConnected)
                    {
                        this.playerName1.Text = User.Instance.UserEntity.Username;
                        this.Button_ShareOnFacebook.Visible = true;
                    }
                    else
                    {
                        if(currentGameState.gameHub != null)
                        {
                            this.CurrentGameState.gameHub.EndOfGameStatsEvent -= OnEndOfGameStats;
                        }
                    }

                    currentGameState = new OfflineGameState();
                    FonctionsNatives.setCurrentOpponentType((int)OpponentType.LOCAL_PLAYER);
                    FonctionsNatives.setOnlineClientType((int)OnlineClientType.OFFLINE_GAME);
                    
                    if (isOnlineGame)
                    {
                        var gameManager = Program.unityContainer.Resolve<GameManager>();

                        var players = gameManager.CurrentOnlineGame?.Players;
                        if (players != null)
                        {
                            this.playerName2.Text = players[0].Id == User.Instance.UserEntity.Id ? players[1].Username : players[0].Username;
                        }
                        this.pointsNb.Visible = true;
                        this.label3.Visible = true;

                        gameManager.CurrentOnlineGame = null;
                    }
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
            if (!this.Panel_EndBack.Visible && FonctionsNatives.isGameStarted())
            {
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
                    Program.FormManager.CurrentForm = Program.HomeMenu;
                    Program.HomeMenu.ChangeViewTo(Program.unityContainer.Resolve<MainMenuViewModel>());
                    return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        /*protected  bool ProcessCmdKey(Keys keyData)
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
                    Program.FormManager.CurrentForm = Program.HomeMenu;
                    Program.HomeMenu.ChangeViewTo(Program.unityContainer.Resolve<MainMenuViewModel>());
                    return true;
            }
            return true;
        }*/
        public AbstractGameState CurrentGameState
        {
            get => currentGameState;
            set => currentGameState = value;
        }

        private void Button_ShareOnFacebook_Click(object sender, EventArgs e)
        {
            Browser browser = new Browser();
            browser.Show();
            browser.webBrowser.Navigate("" +
                "https://www.facebook.com/dialog/feed?" +
                "app_id=143581339623947" +
                "&display=popup" +
                "&link=http://tcpc.isomorphis.me/game.html");
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /*
        private class MessageFilter : IMessageFilter
        {
            private bool processingKey = false;
            public QuickPlay Main { get; set; }
            public bool PreFilterMessage(ref Message msg)
            {
                const int WM_KEYDOWN = 0x100;
                const int WM_KEYUP = 0x101;
                if (msg.Msg == WM_KEYDOWN)
                {
                    var keyData = (Keys)msg.WParam;
                    if (Program.FormManager.CurrentForm.GetType() == typeof(QuickPlay))
                    {
                       // Program.QuickPlay.ProcessCmdKey(keyData);
                    }else if (Program.FormManager.CurrentForm.GetType() == typeof(TestMode))
                    {
                        Program.TestMode.ProcessCmdKey(keyData);
                    }else if (Program.FormManager.CurrentForm.GetType() == typeof(Editeur))
                    {
                        Program.Editeur.ProcessCmdKey(keyData);
                    }
                    return false; // Process keys before return
                        
                }
                else if (msg.Msg == WM_KEYUP)
                {
                //    var keyData = (Keys)msg.WParam;
         
                    // Program.QuickPlay.ProcessCmdKey(keyData);
                    return false; // Process keys before return
                        
                }
                return false;
            }
        }*/

    }
}
