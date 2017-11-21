using InterfaceGraphique.Controls.WPF.Chat;
using InterfaceGraphique.Controls.WPF.Editor;
using InterfaceGraphique.Controls.WPF.Matchmaking;
using InterfaceGraphique.Controls.WPF.ConnectServer;
using Microsoft.Practices.Unity;
using InterfaceGraphique.Controls.WPF.Tournament;
using InterfaceGraphique.Controls.WPF.Home;
using InterfaceGraphique.Controls.WPF.Authenticate;
using InterfaceGraphique.Controls.WPF.Signup;
using InterfaceGraphique.Controls.WPF.Friends;
using InterfaceGraphique.Controls.WPF.Chat.Channel;
using InterfaceGraphique.Controls.WPF.UserProfile;
using InterfaceGraphique.Controls.WPF.Store;
using InterfaceGraphique.Controls.WPF.Tutorial;

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

        public ChannelViewModel ChannelViewModel
        {
            get { return container.Resolve<ChannelViewModel>(); }
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

        public JoinChannelViewModel JoinChannelViewModel
        {
            get { return container.Resolve<JoinChannelViewModel>(); }
        }

        public JoinChannelListViewModel JoinChannelListViewModel
        {
            get { return container.Resolve<JoinChannelListViewModel>(); }
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

        public FriendListViewModel FriendListViewModel => container.Resolve<FriendListViewModel>();

        public FriendListItemViewModel FriendListItemViewModel => container.Resolve<FriendListItemViewModel>();

        public UserProfileViewModel UserProfileViewModel => container.Resolve<UserProfileViewModel>();

        public StoreViewModel StoreViewModel => container.Resolve<StoreViewModel>();

        public AddUserViewModel AddUserViewModel => container.Resolve<AddUserViewModel>();
        public TutorialViewModel TutorialViewModel => container.Resolve<TutorialViewModel>();
        public EditorUsersViewModel EditorUsersViewModel => container.Resolve<EditorUsersViewModel>();
        public CreateMapViewModel CreateMapViewModel => container.Resolve<CreateMapViewModel>();


    }
}
