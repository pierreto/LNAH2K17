using InterfaceGraphique.Controls.WPF.Chat;
using InterfaceGraphique.Controls.WPF.Editor;
using InterfaceGraphique.Controls.WPF.Matchmaking;
using InterfaceGraphique.Controls.WPF.ConnectServer;
using Microsoft.Practices.Unity;
using InterfaceGraphique.Controls.WPF.Tournament;
using InterfaceGraphique.Controls.WPF.Home;
using InterfaceGraphique.Controls.WPF.Authenticate;
using InterfaceGraphique.Controls.WPF.Signup;
using InterfaceGraphique.Controls.WPF.Chat.Channel;

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

        public SignupViewModel SignupViewModel
        {
            get { return container.Resolve<SignupViewModel>(); }
        }


        public ChatListViewModel ChatListViewModel
        {
            get { return container.Resolve<ChatListViewModel>(); }
        }

        public ChatListItemViewModel ChatListItemViewModel
        {
            get { return container.Resolve<ChatListItemViewModel>(); }
        }
        public ChatViewModel ChatViewModel
        {
            get { return container.Resolve<ChatViewModel>(); }
        }
        public MatchmakingViewModel MatchmakingViewModel
        {
            get { return container.Resolve<MatchmakingViewModel>(); }
        }
        public TournamentViewModel TournamentViewModel
        {
            get { return container.Resolve<TournamentViewModel>(); }
        }

        public EditorViewModel EditorViewModel => container.Resolve<EditorViewModel>();

    }
}
