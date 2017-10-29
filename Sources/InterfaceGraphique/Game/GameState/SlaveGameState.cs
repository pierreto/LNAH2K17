using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Entities;
using InterfaceGraphique.Entities.Message;

namespace InterfaceGraphique.Game.GameState
{
    public class SlaveGameState : AbstractGameState
    {

        private GameHub gameHub;
        private bool gameHasEnded = false;
        
        protected GameSlaveData LastSlavePositionSent { get; set; }

        public SlaveGameState(GameHub gameHub)
        {
            this.gameHub = gameHub;
            this.LastSlavePositionSent = new GameSlaveData();
        }

        public override void InitializeGameState(GameEntity gameEntity)
        {
            FonctionsNatives.setOnlineClientType((int)OnlineClientType.SLAVE);

            StringBuilder player1Name = new StringBuilder(gameEntity.Slave.Username.Length);
            StringBuilder player2Name = new StringBuilder(gameEntity.Master.Username.Length);
            player1Name.Append(gameEntity.Slave.Username);
            player2Name.Append(gameEntity.Master.Username);
            FonctionsNatives.setPlayerNames(player1Name, player2Name);

            this.gameHub.InitializeSlaveGameHub(gameEntity.GameId);
            this.gameHub.NewMasterPositions += OnNewGamePositions;
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

            //double totalMillisec = (DateTime.Now - ElapsedTime).TotalMilliseconds;

            //if (totalMillisec >= SERVER_INTERVAL)
            //{
                ElapsedTime = DateTime.Now;
                float[] slavePosition = new float[3];
                FonctionsNatives.getSlavePosition(slavePosition);
                GameSlaveData gameData = new GameSlaveData
                {
                    SlavePosition = slavePosition
                };

                if (!IsSamePosition(gameData.SlavePosition, LastSlavePositionSent.SlavePosition))
                {
                    LastSlavePositionSent = gameData;
                    this.gameHub.SendGameData(gameData);
                }
            //}
        }

        private bool IsSamePosition(float[] position1, float[] position2)
        {
            for (int i = 0; i < 3; i++)
            {
                if (!(position1[i] == position2[i]))
                {
                    return false;
                }
            }
            return true;
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
                Program.FormManager.CurrentForm = Program.OnlineTournament;
            }
        }

        private void OnNewGamePositions(GameMasterData gameData)
        {
            log(DateTime.Now.ToLongTimeString() + " Master: " + PrintPosition(gameData.MasterPosition)
                + " Puck: " + PrintPosition(gameData.PuckPosition));
            if (!gameHasEnded && gameData.MasterPosition != null && gameData.PuckPosition != null)
            {
                FonctionsNatives.setSlaveGameElementPositions(gameData.MasterPosition, gameData.PuckPosition);
            }
        }

        private string PrintPosition(float[] position)
        {
            return string.Join(",", position);
        }

        private void log(string message)
        {
            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"C:/TEMP/log.txt", true))
            {
                file.WriteLine(message);
            }
        }

    }
}
