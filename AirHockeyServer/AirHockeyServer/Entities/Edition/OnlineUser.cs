using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AirHockeyServer.Entities.EditionCommand
{
    public class OnlineUser
    {
        public string Username { get; set; }
        public string HexColor { get; set; }
        public string ProfilePicture { get; set; }
        public List<string> UuidsSelected { get; set; }

        [JsonIgnore]
        public string CurrentMapId { get; set; }


        public OnlineUser()
        {
            UuidsSelected=new List<string>();
        }
    }
}
