using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Controls.WPF.Chat;
using InterfaceGraphique.Controls.WPF.Matchmaking;
using InterfaceGraphique.Controls.WPF.ConnectServer;
using Microsoft.Practices.Unity;

namespace InterfaceGraphique.Controls.WPF
{
    public class ViewModelLocator
    {
        private static ViewModelBase instance;

        private UnityContainer container;

        public ViewModelLocator()
        {
            container = Program.unityContainer;
        }

        public ConnectServerViewModel ConnectServerViewModel
        {
            get { return container.Resolve<ConnectServerViewModel>(); }
        }

        public ChatViewModel ChatViewModel
        {
            get { return container.Resolve<ChatViewModel>(); }
        }
        public MatchmakingViewModel MatchmakingViewModel
        {
            get { return container.Resolve<MatchmakingViewModel>(); }
        }
    }
}
