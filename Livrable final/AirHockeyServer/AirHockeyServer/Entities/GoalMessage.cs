using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Web;

namespace AirHockeyServer.Entities
{
    public class GoalMessage : Entity
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