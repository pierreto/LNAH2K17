using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Web;

namespace AirHockeyServer.Entities
{
    public class GameDataMessage : Entity
    {
        private int[] slavePosition;
        private int[] masterPosition;
        private int[] puckPosition;
    }
}