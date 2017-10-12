using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceGraphique.Entities
{
    public class GoalMessage
    {
        private int playerNumber;

        public GoalMessage(int playerNumber)
        {
            this.playerNumber = playerNumber;
        }

        public int PlayerNumber
        {
            get => playerNumber;
            set => playerNumber = value;
        }
    }
}
