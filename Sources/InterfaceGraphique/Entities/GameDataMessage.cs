using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceGraphique.Entities
{
    public class GameDataMessage
    {
        private float[] slavePosition;
        private float[] masterPosition;
        private float[] puckPosition;

        public GameDataMessage()
        {
            this.slavePosition = new float[3];
            this.masterPosition = new float[3];
            this.puckPosition = new float[3];
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
