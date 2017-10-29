using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceGraphique.Entities.Message
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
