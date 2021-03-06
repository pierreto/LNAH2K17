﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Entities;
using InterfaceGraphique.Services;
using System.Drawing;
using Microsoft.Practices.Unity;
using InterfaceGraphique.CommunicationInterface.WaitingRooms;
using InterfaceGraphique.Controls.WPF.Friends;

namespace InterfaceGraphique.Game.GameState
{
    public class SlaveGameState : AbstractGameState
    {

        //private GameHub gameHub;
        private bool gameHasEnded = false;

        public MapService MapService { get; set; }
        public GameManager GameManager { get; }

        public SlaveGameState(GameHub gameHub, MapService mapService, GameManager gameManager)
        {
            this.gameHub = gameHub;
            MapService = mapService;
            GameManager = gameManager;
        }

        public override async void InitializeGameState(GameEntity gameEntity)
        {
            FonctionsNatives.setOnlineClientType((int)OnlineClientType.SLAVE);
            FonctionsNatives.setCurrentOpponentType((int)OpponentType.ONLINE_PLAYER);

            this.gameHub.InitialiseGame(gameEntity.GameId);

            gameHasEnded = false;

            StringBuilder player1Name = new StringBuilder(gameEntity.Slave.Username.Length);
            StringBuilder player2Name = new StringBuilder(gameEntity.Master.Username.Length);
            player1Name.Append(gameEntity.Slave.Username);
            player2Name.Append(gameEntity.Master.Username);
            FonctionsNatives.setPlayerNames(player1Name, player2Name);

            float[] playerColor = new float[4] { Color.White.R, Color.White.G, Color.White.B, Color.White.A };
            FonctionsNatives.setPlayerColors(playerColor, playerColor);
            
            this.gameHub.NewPositions += OnNewGamePositions;
            this.gameHub.NewGoal += OnNewGoal;
            this.gameHub.GameOver += EndGame;
            this.gameHub.DisconnectedEvent += OnDisconnexion;

            selectedMap = gameEntity.SelectedMap;


        }

        private void OnNewGoal(GoalMessage goalMessage)
        {
            if(goalMessage.PlayerNumber == GameManager.CurrentOnlineGame.Players.Where(x => x.Id != User.Instance.UserEntity.Id).First().Id)
            {
                Program.QuickPlay.BeginInvoke(new MethodInvoker(delegate
                {
                    FonctionsNatives.slaveGoal();
                }));
            }
            else if (goalMessage.PlayerNumber == User.Instance.UserEntity.Id)
            {
                Program.QuickPlay.BeginInvoke(new MethodInvoker(delegate
                {
                    FonctionsNatives.masterGoal();
                }));
            }
        }

        public override void MettreAJour(double tempsInterAffichage, int neededGoalsToWin)
        {
            FonctionsNatives.animer(tempsInterAffichage);
            FonctionsNatives.dessinerOpenGL();

            float[] slavePosition = new float[3];
            FonctionsNatives.getSlavePosition(slavePosition);
            Task.Run(() => this.gameHub.SendSlavePosition(slavePosition));
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
        public override void MouseMoved(object sender, MouseEventArgs e)
        {

            FonctionsNatives.opponentMouseMove(e.Location.X, e.Location.Y);

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
        public override void KeyDownEvent(object sender, KeyEventArgs e)
        {
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
        public override void KeyUpEvent(object sender, KeyEventArgs e)
        {
        }

        ////////////////////////////////////////////////////////////////////////
        ///
        /// Cette fonction s'occupe de gérer la fin de partie.
        ///
        /// @return Void 
        ///
        ////////////////////////////////////////////////////////////////////////
        public override void EndGame()
        {
            gameHasEnded = true;
            Program.QuickPlay.Invoke(new MethodInvoker(async () =>
            {
                Program.QuickPlay.GetReplayButton().Visible = false;
            }));
                Program.QuickPlay.EndGame(true);
                Program.QuickPlay.UnsuscribeEventHandlers();
            FonctionsNatives.setGameEnded();

            User.Instance.UserEntity.IsPlaying = false;
            Program.unityContainer.Resolve<FriendListViewModel>().OnPropertyChanged("CanShowPlay");
            if (Program.unityContainer.Resolve<FriendListViewModel>().FriendList != null)
            {
                foreach (FriendListItemViewModel flivm in Program.unityContainer.Resolve<FriendListViewModel>().FriendList)
                {
                    flivm.OnPropertyChanged("CanSendPlay");
                }
            }

            this.gameHub.NewPositions -= OnNewGamePositions;
            this.gameHub.NewGoal -= OnNewGoal;
            this.gameHub.GameOver -= EndGame;
            this.gameHub.DisconnectedEvent -= OnDisconnexion;

            if (IsOnlineTournementMode)
            {
                Program.OnlineTournament.Invoke(new MethodInvoker(() =>
                {
                    Program.FormManager.CurrentForm = Program.OnlineTournament;
                }));
            }
        }

        private void OnNewGamePositions(GameDataMessage gameData)
        {
            if (Program.QuickPlay.CurrentGameState.GameInitialized && !gameHasEnded && gameData.MasterPosition != null && gameData.SlavePosition != null && gameData.PuckPosition != null)
            {
                Program.QuickPlay.BeginInvoke(new MethodInvoker(delegate
                {
                    FonctionsNatives.setSlaveGameElementPositions(gameData.SlavePosition, gameData.MasterPosition,
                        gameData.PuckPosition);
                }));
            }
        }

        public override void ApplyTextures()
        {
            if (User.Instance.IsConnected)
            {
                var gameManager = Program.unityContainer.Resolve<GameManager>();
                var textures = gameManager.Textures;

                if (textures != null)
                {
                    if (textures[0] != null)
                    {
                        FonctionsNatives.setOpponentPlayerSkin(textures[0]);
                    }
                    if (textures[1] != null)
                    {
                        FonctionsNatives.setLocalPlayerSkin(textures[1]);
                    }
                }
            }
        }

    }
}
