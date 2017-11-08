using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirHockeyServer.Entities.EditionCommand
{
    public class OnlineUser
    {
        public string Username { get; set; }
        public string HexColor { get; set; }

        public List<string> UuidsSelected { get; set; }

        public OnlineUser()
        {
            UuidsSelected=new List<string>();
        }
    }
}
