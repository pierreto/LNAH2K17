using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Integration;

namespace InterfaceGraphique.Controls
{
    class WPFChatHost : ElementHost
    {

        protected WPFChatView wpfChat = new WPFChatView();

        public WPFChatHost()
        {
            base.Child = wpfChat;
        }
    }
}
