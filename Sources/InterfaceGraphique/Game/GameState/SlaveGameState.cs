using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Entities;

namespace InterfaceGraphique.Game.GameState
{
    public class SlaveGameState : AbstractGameState
    {

        //private GameHub gameHub;
        private bool gameHasEnded = false;

        public SlaveGameState(GameHub gameHub)
        {
            this.gameHub = gameHub;
        }

        public override void InitializeGameState(GameEntity gameEntity)
        {
            FonctionsNatives.setOnlineClientType((int)OnlineClientType.SLAVE);
            FonctionsNatives.setCurrentOpponentType((int)OpponentType.ONLINE_PLAYER);

            StringBuilder player1Name = new StringBuilder(gameEntity.Slave.Username.Length);
            StringBuilder player2Name = new StringBuilder(gameEntity.Master.Username.Length);
            player1Name.Append(gameEntity.Slave.Username);
            player2Name.Append(gameEntity.Master.Username);
            FonctionsNatives.setPlayerNames(player1Name, player2Name);

            gameHasEnded = false;

            this.gameHub.InitializeSlaveGameHub(gameEntity.GameId);
            this.gameHub.NewPositions += OnNewGamePositions;
            this.gameHub.NewGoal += OnNewGoal;
            this.gameHub.NewGameOver += EndGame;
        }

        private void OnNewGoal(GoalMessage goalMessage)
        {
            if (goalMessage.PlayerNumber == 1)
            {
                FonctionsNatives.slaveGoal();
            }
            else if (goalMessage.PlayerNumber == 2)
            {
                FonctionsNatives.masterGoal();
            }
        }

        public override void MettreAJour(double tempsInterAffichage, int neededGoalsToWin)
        {
            FonctionsNatives.animer(tempsInterAffichage);
            FonctionsNatives.dessinerOpenGL();

            float[] slavePosition = new float[3];
            FonctionsNatives.getSlavePosition(slavePosition);
            Task.Run(() =>this.gameHub.SendSlavePosition(slavePosition));
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
            Program.QuickPlay.EndGame();
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
            if (!gameHasEnded && gameData.MasterPosition != null && gameData.SlavePosition != null && gameData.PuckPosition != null)
            {
                FonctionsNatives.setSlaveGameElementPositions(gameData.SlavePosition, gameData.MasterPosition, gameData.PuckPosition);
            }
        }

    }
}
