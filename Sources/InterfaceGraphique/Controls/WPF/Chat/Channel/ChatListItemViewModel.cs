using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceGraphique.Controls.WPF.Chat.Channel
{
    public class ChatListItemViewModel : ViewModelBase
    {

        public string Name { get; set; }

        public string Initials { get; set; }

        public string ProfilePictureRGB { get; set; }

        public bool IsSelected { get; set; }

        public bool NewContentAvailable { get; set; }

        public override void InitializeViewModel()
        {
           // throw new NotImplementedException();
        }
    }
}
