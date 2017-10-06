using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceGraphique.Entities
{
    public class GoalMessage
    {
        private string playerNameWhoScored;

        public GoalMessage(string playerNameWhoScored)
        {
            this.playerNameWhoScored = playerNameWhoScored;
        }
    }
}
