using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InterfaceGraphique.Entities;
using InterfaceGraphique.CommunicationInterface;

namespace InterfaceGraphique.Game.GameState
{
    class OfflineGameState : AbstractGameState
    {
        public override void InitializeGameState(GameEntity gameEntity)
        {
            FonctionsNatives.setOnlineClientType((int)OnlineClientType.OFFLINE_GAME);
        }

        public override void MettreAJour(double tempsInterAffichage,int neededGoalsToWin)
        {
            FonctionsNatives.moveMaillet();
            FonctionsNatives.animer(tempsInterAffichage);
            FonctionsNatives.dessinerOpenGL();

            if (FonctionsNatives.isGameOver(neededGoalsToWin) == 1)
                EndGame();
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
            if (e.KeyCode == this.keyUp && !this.MoveUpKeyDown)
            {
                this.MoveUpKeyDown = true;
                this.MoveDownKeyDown = false;
                FonctionsNatives.setSpeedYMaillet(GlobalVariables.speedMaillet);
            }
            if (e.KeyCode == this.KeyLeft && !this.MoveLeftKeyDown)
            {
                this.MoveLeftKeyDown = true;
                this.MoveRightKeyDown = false;
                FonctionsNatives.setSpeedXMaillet(-GlobalVariables.speedMaillet);
            }
            if (e.KeyCode == this.KeyDown && !this.MoveDownKeyDown)
            {
                this.MoveDownKeyDown = true;
                this.MoveUpKeyDown = false;
                FonctionsNatives.setSpeedYMaillet(-GlobalVariables.speedMaillet);
            }
            if (e.KeyCode == this.KeyRight && !this.MoveRightKeyDown)
            {
                this.MoveRightKeyDown = true;
                this.MoveLeftKeyDown = false;
                FonctionsNatives.setSpeedXMaillet(GlobalVariables.speedMaillet);
            }
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
            if (e.KeyCode == keyUp && moveUpKeyDown)
            {
                moveUpKeyDown = false;
                FonctionsNatives.setSpeedYMaillet(0);
            }
            if (e.KeyCode == keyLeft && moveLeftKeyDown)
            {
                moveLeftKeyDown = false;
                FonctionsNatives.setSpeedXMaillet(0);
            }
            if (e.KeyCode == keyDown && moveDownKeyDown)
            {
                moveDownKeyDown = false;
                FonctionsNatives.setSpeedYMaillet(0);
            }
            if (e.KeyCode == keyRight && moveRightKeyDown)
            {
                moveRightKeyDown = false;
                FonctionsNatives.setSpeedXMaillet(0);
            }
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
            Program.LobbyHost.Invoke(new MethodInvoker(async () =>
            {
                Program.QuickPlay.GetReplayButton().Visible = true;
            }));
            Program.QuickPlay.EndGame(false);
            User.Instance.UserEntity.IsPlaying = false;
        }
    }
}
