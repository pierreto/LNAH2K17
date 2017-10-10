﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Entities;

namespace InterfaceGraphique.Game.GameState
{
    public class MasterGameState : AbstractGameState
    {

        private GameHub gameHub;
        private bool gameHasEnded = false;
        private FonctionsNatives.GoalCallback callback;

        public MasterGameState(GameHub gameHub)
        {
            this.gameHub = gameHub;
            this.callback =
                (player) =>
                {
                    Console.WriteLine("Player {0} scored", player);
                    this.gameHub.SendGoal(player);
                };
        }

        public override void InitializeGameState(GameEntity gameEntity)
        {
            FonctionsNatives.setOnlineClientType((int) OnlineClientType.MASTER);
            this.gameHub.InitializeMasterGameHub(gameEntity.GameId);
            this.gameHub.NewPositions += OnNewGamePositions;

        
            FonctionsNatives.setOnGoalCallback(callback);
        }

        public override void MettreAJour(double tempsInterAffichage, int neededGoalsToWin)
        {
            if (FonctionsNatives.isGameOver(neededGoalsToWin) == 1)
            {
                EndGame();
                return;
            }
            FonctionsNatives.moveMaillet();
            FonctionsNatives.animer(tempsInterAffichage);
            FonctionsNatives.dessinerOpenGL();

            float[] slavePosition = new float[3];
            float[] masterPosition = new float[3];
            float[] puckPosition = new float[3];

            FonctionsNatives.getGameElementPositions(slavePosition,masterPosition,puckPosition);

             gameHub.SendMasterPosition(slavePosition, masterPosition, puckPosition);
       

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
        public override void EndGame() {
            gameHasEnded = true;
            gameHub.SendGameOver();
            Program.QuickPlay.EndGame(); 
        }
        private void OnNewGamePositions(GameDataMessage gameData)
        {
            if (!gameHasEnded)
            {
                FonctionsNatives.setMasterGameElementPositions(gameData.SlavePosition);
            }
        }
    }
}
