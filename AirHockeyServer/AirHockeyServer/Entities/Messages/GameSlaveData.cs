using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirHockeyServer.Entities.Messages
{
    public class GameSlaveData
    {
        public float[] SlavePosition { get; set; }

        public GameSlaveData()
        {
            SlavePosition = new float[3];
        }
    }
}