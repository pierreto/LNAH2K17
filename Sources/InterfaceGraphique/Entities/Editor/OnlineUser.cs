using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceGraphique.Entities.Editor
{
    public class OnlineUser
    {
        public string Username { get; set; }
        public string HexColor { get; set; }
        public List<string> UuidsSelected { get; set; }
        public string ProfilePicture { get; set; }
    }
}
