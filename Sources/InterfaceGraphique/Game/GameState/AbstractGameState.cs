using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InterfaceGraphique.Entities;
using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.CommunicationInterface.WaitingRooms;
using Microsoft.Practices.Unity;

namespace InterfaceGraphique.Game.GameState
{
    public abstract class AbstractGameState
    {
        public GameHub gameHub;
        public MapEntity selectedMap;
        public bool gameHasEnded = false;
        protected bool moveUpKeyDown = false;
        protected bool moveDownKeyDown = false;
        protected bool moveLeftKeyDown = false;
        protected bool moveRightKeyDown = false;

        protected Keys keyUp = Keys.W;
        protected Keys keyDown = Keys.S;
        protected Keys keyLeft = Keys.A;
        protected Keys keyRight = Keys.D;

        protected int neededGoalsToWin = 2;
        protected bool isTournementMode;

        protected string mapFilePath;


        // Accessors
        public abstract void InitializeGameState(GameEntity gameEntity);
        public abstract void MouseMoved(object sender, MouseEventArgs e);
        public abstract void KeyDownEvent(object sender, KeyEventArgs e);
        public abstract void KeyUpEvent(object sender, KeyEventArgs e);
        public abstract void EndGame();
        public abstract void MettreAJour(double tempsInterAffichage, int neededGoalsToWin);


        public bool MoveUpKeyDown
        {
            get => moveUpKeyDown;
            set => moveUpKeyDown = value;
        }

        public bool MoveDownKeyDown
        {
            get => moveDownKeyDown;
            set => moveDownKeyDown = value;
        }

        public bool MoveLeftKeyDown
        {
            get => moveLeftKeyDown;
            set => moveLeftKeyDown = value;
        }

        public bool MoveRightKeyDown
        {
            get => moveRightKeyDown;
            set => moveRightKeyDown = value;
        }

        public Keys KeyUp
        {
            get => keyUp;
            set => keyUp = value;
        }

        public Keys KeyDown
        {
            get => keyDown;
            set => keyDown = value;
        }

        public Keys KeyLeft
        {
            get => keyLeft;
            set => keyLeft = value;
        }

        public Keys KeyRight
        {
            get => keyRight;
            set => keyRight = value;
        }

        public int NeededGoalsToWin
        {
            get => neededGoalsToWin;
            set => neededGoalsToWin = value;
        }

        public bool IsTournementMode
        {
            get => isTournementMode;
            set => isTournementMode = value;
        }

        public bool IsOnlineTournementMode
        {
            get;
            set;
        }

        public string MapFilePath
        {
            get => mapFilePath;
            set => mapFilePath = value;
        }

        public bool IsOnline { get; set; }

        public GameEntity OnlineGame { get; set; }

        public bool GameInitialized { get; set; }

        protected void OnDisconnexion()
        {
            if(gameHasEnded)
            {
                return;
            }

            Program.QuickPlay.Invoke(new MethodInvoker(() =>
            {
                Program.QuickPlay.ReplacePlayerByAI();
            }));

            MessageBox.Show(
                @"Votre adversaire n'est plus en ligne et vient d'être remplacé par un joueur virtuel. Vous pouvez reprendre la partie en appuyant sur Esc.",
                @"Information",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public virtual void ApplyTextures()
        {
            if (User.Instance.IsConnected)
            {
                var gameManager = Program.unityContainer.Resolve<GameManager>();
                var textures = gameManager.Textures;

                if (textures != null)
                {
                    if (textures[0] != null)
                    {
                        FonctionsNatives.setLocalPlayerSkin(textures[0]);
                    }
                    if (textures[1] != null)
                    {
                        FonctionsNatives.setOpponentPlayerSkin(textures[1]);
                    }
                }
            }
        }
    }
}
