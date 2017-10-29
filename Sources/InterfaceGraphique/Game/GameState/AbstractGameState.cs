using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InterfaceGraphique.Entities;

namespace InterfaceGraphique.Game.GameState
{
    public abstract class AbstractGameState
    {
        protected DateTime ElapsedTime = new DateTime();
        protected const double SERVER_INTERVAL = 100;

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
    }
}
