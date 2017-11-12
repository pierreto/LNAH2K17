using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Entities;
using InterfaceGraphique.Services;

namespace InterfaceGraphique.Game.GameState
{
    public class MasterGameState : AbstractGameState
    {

        //private GameHub gameHub;
        private bool gameHasEnded = false;
        private FonctionsNatives.GoalCallback callback;
        private int ELapsedTime = 0;
        private const int SERVER_INTERVAL = 5;

        public MapService MapService { get; set; }

        public MasterGameState(GameHub gameHub, MapService mapService)
        {
            this.gameHub = gameHub;
            MapService = mapService;
            this.callback =
                (player) =>
                {
                    Console.WriteLine("Player {0} scored", player);
                    Task.Run(() =>this.gameHub.SendGoal(player));
                };
        }

        public override async void InitializeGameState(GameEntity gameEntity)
        {
            FonctionsNatives.setOnlineClientType((int) OnlineClientType.MASTER);
            FonctionsNatives.setCurrentOpponentType((int)OpponentType.ONLINE_PLAYER);

            this.gameHub.InitializeMasterGameHub(gameEntity.GameId);
            this.gameHub.NewPositions += OnNewGamePositions;

            gameHasEnded = false;
            FonctionsNatives.setOnGoalCallback(callback);
            
            base.LoadOnlineMap(gameEntity.SelectedMap);

            StringBuilder player1Name = new StringBuilder(gameEntity.Master.Username.Length);
            StringBuilder player2Name = new StringBuilder(gameEntity.Slave.Username.Length);
            player1Name.Append(gameEntity.Master.Username);
            player2Name.Append(gameEntity.Slave.Username);
            FonctionsNatives.setPlayerNames(player1Name, player2Name);
        }

        public override void MettreAJour(double tempsInterAffichage, int neededGoalsToWin)
        {
            if (!gameHasEnded && FonctionsNatives.isGameOver(neededGoalsToWin) == 1)
            {
                EndGame();
                gameHasEnded = true;
                return;
            }
            FonctionsNatives.moveMaillet();
            FonctionsNatives.animer(tempsInterAffichage);
            FonctionsNatives.dessinerOpenGL();

            //if (ELapsedTime >= SERVER_INTERVAL)
            //{
                ELapsedTime = 0;

                float[] slavePosition = new float[3];
                float[] masterPosition = new float[3];
                float[] puckPosition = new float[3];

                FonctionsNatives.getGameElementPositions(slavePosition,masterPosition,puckPosition);
            
                Task.Run(() =>gameHub.SendGameData(slavePosition, masterPosition, puckPosition));
            //}


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
            Task.Run(() => gameHub.SendGameOver());
            Program.QuickPlay.EndGame();
            if(IsOnlineTournementMode)
            {
                Program.OnlineTournament.Invoke(new MethodInvoker(() =>
                {
                    Program.FormManager.CurrentForm = Program.OnlineTournament;
                }));
            }
        }

        private void OnNewGamePositions(GameDataMessage gameData)
        {
            if (!gameHasEnded && gameData.SlavePosition != null)
            {
                FonctionsNatives.setMasterGameElementPositions(gameData.SlavePosition);
            }
        }
    }
}
