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

        private GameHub gameHub;
        private bool gameHasEnded = false;

        public SlaveGameState(GameHub gameHub)
        {
            this.gameHub = gameHub;
        }

        public override void InitializeGameState(GameEntity gameEntity)
        {
            FonctionsNatives.setOnlineClientType((int)OnlineClientType.SLAVE);

            this.gameHub.InitializeSlaveGameHub(gameEntity.GameId);
            this.gameHub.NewPositions += OnNewGamePositions;
            this.gameHub.NewGameOver += EndGame;
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
            float[] slavePosition = new float[3];
            FonctionsNatives.getSlavePosition(slavePosition);
            this.gameHub.SendSlavePosition(slavePosition);
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
        }

        private void OnNewGamePositions(GameDataMessage gameData)
        {
            if (!gameHasEnded)
            {
                 FonctionsNatives.setSlaveGameElementPositions(gameData.SlavePosition,gameData.MasterPosition,gameData.PuckPosition);
            }
        }

    }
}
