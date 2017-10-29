using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceGraphique.Entities.Message
{
    public class GameMasterData
    {
        public float[] MasterPosition { get; set; }

        public float[] PuckPosition { get; set; }

        public GameMasterData()
        {
            this.MasterPosition = new float[3];
            this.PuckPosition = new float[3];
        }
    }
}
