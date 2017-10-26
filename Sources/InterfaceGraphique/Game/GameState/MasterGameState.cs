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
            FonctionsNatives.setOnlineClientType((int)OnlineClientType.MASTER);
            FonctionsNatives.setCurrentOpponentType((int)OpponentType.ONLINE_PLAYER);

            this.gameHub.InitializeMasterGameHub(gameEntity.GameId);
            this.gameHub.NewPositions += OnNewGamePositions;


            FonctionsNatives.setOnGoalCallback(callback);

            StringBuilder player1Name = new StringBuilder(gameEntity.Master.Username.Length);
            StringBuilder player2Name = new StringBuilder(gameEntity.Slave.Username.Length);
            player1Name.Append(gameEntity.Master.Username);
            player2Name.Append(gameEntity.Slave.Username);
            FonctionsNatives.setPlayerNames(player1Name, player2Name);
        }

        private int elapsedTime = 0;
        private const int UPDATE_SERVER_INTERVAL = 5;
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

            if (elapsedTime >= UPDATE_SERVER_INTERVAL)
            {
                elapsedTime = 0;
                FonctionsNatives.getGameElementPositions(slavePosition, masterPosition, puckPosition);

                gameHub.SendGameData(slavePosition, masterPosition, puckPosition);
            }

            elapsedTime++;


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
        public override void EndGame()
        {
            gameHasEnded = true;
            gameHub.SendGameOver();
            Program.QuickPlay.EndGame();
            if (IsOnlineTournementMode)
            {
                Program.FormManager.CurrentForm = Program.OnlineTournament;
            }
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
