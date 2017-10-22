using InterfaceGraphique.Controls.WPF.Chat;
using InterfaceGraphique.Controls.WPF.Matchmaking;
using InterfaceGraphique.Controls.WPF.ConnectServer;
using Microsoft.Practices.Unity;
using InterfaceGraphique.Controls.WPF.Home;
using InterfaceGraphique.Controls.WPF.Authenticate;

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

        public HomeViewModel HomeViewModel
        {
            get { return container.Resolve<HomeViewModel>(); }
        }

        public ConnectServerViewModel ConnectServerViewModel
        {
            get { return container.Resolve<ConnectServerViewModel>(); }
        }

        public AuthenticateViewModel AuthenticateViewModel
        {
            get { return container.Resolve<AuthenticateViewModel>(); }
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
