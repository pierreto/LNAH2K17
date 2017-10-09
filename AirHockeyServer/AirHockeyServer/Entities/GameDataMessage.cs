using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Web;

namespace AirHockeyServer.Entities
{
    public class GameDataMessage : Entity
    {
        private float[] slavePosition;
        private float[] masterPosition;
        private float[] puckPosition;
        public GameDataMessage()
        {
        }
        public GameDataMessage(float[] slavePosition, float[] masterPosition, float[] puckPosition)
        {
            this.slavePosition = slavePosition;
            this.masterPosition = masterPosition;
            this.puckPosition = puckPosition;
        }
        public GameDataMessage(float[] slavePosition)
        {
            this.slavePosition = slavePosition;
        }

        public float[] SlavePosition
        {
            get => slavePosition;
            set => slavePosition = value;
        }

        public float[] MasterPosition
        {
            get => masterPosition;
            set => masterPosition = value;
        }

        public float[] PuckPosition
        {
            get => puckPosition;
            set => puckPosition = value;
        }
    }
}