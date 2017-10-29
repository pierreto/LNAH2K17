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
    public class MasterGameState : AbstractGameState
    {
        private GameHub gameHub;
        private bool gameHasEnded = false;
        private FonctionsNatives.GoalCallback callback;

        private GameMasterData LastGameDataSent { get; set; }
        
        public MasterGameState(GameHub gameHub)
        {
            this.gameHub = gameHub;
            this.LastGameDataSent = new GameMasterData();
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
            this.gameHub.NewSlavePositions += OnNewGamePositions;


            FonctionsNatives.setOnGoalCallback(callback);

            StringBuilder player1Name = new StringBuilder(gameEntity.Master.Username.Length);
            StringBuilder player2Name = new StringBuilder(gameEntity.Slave.Username.Length);
            player1Name.Append(gameEntity.Master.Username);
            player2Name.Append(gameEntity.Slave.Username);
            FonctionsNatives.setPlayerNames(player1Name, player2Name);
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

            double totalMillisec = (DateTime.Now - ElapsedTime).TotalMilliseconds;

            if (totalMillisec >= SERVER_INTERVAL)
            {
                ElapsedTime = DateTime.Now;

                float[] slavePosition = new float[3];
                float[] masterPosition = new float[3];
                float[] puckPosition = new float[3];
                
                FonctionsNatives.getGameElementPositions(slavePosition, masterPosition, puckPosition);
                GameMasterData gameData = new GameMasterData
                {
                    MasterPosition = masterPosition,
                    PuckPosition = puckPosition
                };

                if(!IsSamePosition(LastGameDataSent.MasterPosition,gameData.MasterPosition) ||
                    !IsSamePosition(LastGameDataSent.PuckPosition , gameData.PuckPosition))
                {
                    LastGameDataSent = gameData;
                    Log(DateTime.Now.ToLongTimeString() + " Master: " + PrintPosition(gameData.MasterPosition)
                        + " Puck: " + PrintPosition(gameData.PuckPosition));
                    gameHub.SendGameData(gameData);
                }
            }
                      }

        private bool IsSamePosition(float[] position1, float[] position2)
        {
            for(int i = 0; i < 3; i++)
            {
                if(!(position1[i] == position2[i]))
                {
                    return false;
                }
            }
            return true;
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

        private void OnNewGamePositions(GameSlaveData gameData)
        {
            if (!gameHasEnded)
            {
                FonctionsNatives.setMasterGameElementPositions(gameData.SlavePosition);
            }
        }

        private string PrintPosition(float[] position)
        {
            return string.Join(",", position);
        }

        private void Log(string message)
        {
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"C:\dev\log.txt", true))
            {
                file.WriteLine(message);
            }
        }
    }
}
