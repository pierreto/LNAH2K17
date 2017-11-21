using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Controls;
using InterfaceGraphique.Controls.WPF;
using InterfaceGraphique.Controls.WPF.Chat;
using InterfaceGraphique.Controls.WPF.Editor;
using InterfaceGraphique.Controls.WPF.Friends;
using InterfaceGraphique.Entities;
using InterfaceGraphique.Menus;
using Microsoft.Practices.Unity;
using Application = System.Windows.Forms.Application;
using InterfaceGraphique.Controls.WPF.Matchmaking;
using InterfaceGraphique.Controls.WPF.Tournament;
using InterfaceGraphique.CommunicationInterface.WaitingRooms;
using InterfaceGraphique.Controls.WPF.Authenticate;
using InterfaceGraphique.Controls.WPF.Home;
using InterfaceGraphique.Controls.WPF.ConnectServer;
using InterfaceGraphique.Controls.WPF.Signup;
using InterfaceGraphique.Services;
using InterfaceGraphique.Controls.WPF.Chat.Channel;
using InterfaceGraphique.Editor;
using InterfaceGraphique.Controls.WPF.UserProfile;
using InterfaceGraphique.Controls.WPF.Store;
using InterfaceGraphique.Controls.WPF.Tutorial;
using InterfaceGraphique.Editor.EditorState;
using InterfaceGraphique.Game.GameState;
using InterfaceGraphique.Managers;

namespace InterfaceGraphique
{
    static class Program {
        private const int NB_IMAGES_PAR_SECONDE = 120;

        public static Object unLock = new Object();
        public static bool peutAfficher = true;

        public static TestChatMenu TestChatMenu { get { return testChatMenu; } }
        public static FormManager FormManager { get { return formManager; } }
        public static MainMenu MainMenu { get { return mainMenu; } }
        public static HomeMenu HomeMenu { get { return homeMenu; } }
        public static Editeur Editeur { get { return editeur; } }
        public static ConfigurationMenu ConfigurationMenu { get { return configurationMenu; } }
        public static QuickPlay QuickPlay { get { return quickPlay; } }
        public static TestMode TestMode { get { return testMode; } }
        public static GeneralProperties GeneralProperties { get { return generalProperties; } }
        public static QuickPlayMenu QuickPlayMenu { get { return quickPlayMenu; } }
        public static TournementMenu TournementMenu { get { return tournementMenu; } }
        public static TournementTree TournementTree { get { return tournementTree; } }
        public static CreditsMenu CreditsMenu { get { return creditsMenu; } }
        public static Panel OpenGLPanel { get { return openGLPanel; } set { openGLPanel = value; } }
        public static LobbyHost LobbyHost { get { return lobbyHost; } set { lobbyHost = value; } }
        public static OnlineTournament OnlineTournament { get { return onlineTournament;  } set { onlineTournament = value; } }
        public static EditorHost EditorHost { get { return editorHost; } set { editorHost = value; } }
        public static UserProfileMenu UserProfileMenu { get { return userProfileMenu; } set { userProfileMenu = value; } }
        public static StoreMenu StoreMenu { get { return storeMenu; } set { storeMenu = value; } } 
        public static TutorialHost TutorialHost { get { return tutorialHost; } set { tutorialHost = value; } } 

        private static TestChatMenu testChatMenu;
        private static FormManager formManager;
        private static MainMenu mainMenu;
        private static HomeMenu homeMenu;
        private static Editeur editeur;
        private static ConfigurationMenu configurationMenu;
        private static QuickPlay quickPlay;
        private static TestMode testMode;
        private static GeneralProperties generalProperties;
        private static QuickPlayMenu quickPlayMenu;
        private static TournementMenu tournementMenu;
        private static TournementTree tournementTree;
        private static CreditsMenu creditsMenu;
        private static LobbyHost lobbyHost;
        private static EditorHost editorHost;
        private static OnlineTournament onlineTournament;
        private static UserProfileMenu userProfileMenu;
        private static StoreMenu storeMenu;
        private static TutorialHost tutorialHost;

        private static Panel openGLPanel;
        //private static Login login;
        private static TimeSpan dernierTemps;
        private static TimeSpan tempsAccumule;
        private static Stopwatch chrono = Stopwatch.StartNew();
        private static TimeSpan tempsEcouleVoulu = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / NB_IMAGES_PAR_SECONDE);

        public static HttpClient client = new HttpClient();

        public static UnityContainer unityContainer;

        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>

        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length != 0)
                if (args[0] == "testsC++")
                {
                    if (FonctionsNatives.executerTests())
                        System.Console.WriteLine("Échec d'un ou plusieurs tests.");
                    else
                        System.Console.WriteLine("Tests réussis.");
                }

            chrono.Start();
            Application.Idle += ExecuterQuandInactif;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);




            // When the app exits
            Application.ApplicationExit += AppExit;

            // Unhandled exceptions for our Application Domain
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            // Unhandled exceptions for the executing UI thread
            Application.ThreadException += ThreadExceptionHandler;

            WPFApplication.Start();

            InitializeUnityDependencyInjection();

            tutorialHost = new TutorialHost();
            editorHost = new EditorHost();

            openGLPanel = new Panel();
            formManager = new FormManager();
            homeMenu = new HomeMenu();
            Editeur.mapManager = unityContainer.Resolve<MapManager>();
            configurationMenu = new ConfigurationMenu();
            quickPlay = new QuickPlay();
            testMode = new TestMode();
            generalProperties = new GeneralProperties();
            testChatMenu = new TestChatMenu();
            creditsMenu = new CreditsMenu();
            lobbyHost = new LobbyHost();
            onlineTournament = new OnlineTournament();
            userProfileMenu = new UserProfileMenu();
            storeMenu = new StoreMenu();

            FonctionsNatives.loadSounds();

            formManager.CurrentForm = homeMenu;

            Application.Run(formManager);



        }

        private static void ThreadExceptionHandler(object sender, ThreadExceptionEventArgs e)
        {
            Debug.Print("Thread exception");

            Debug.Print(e.ToString());
        }

        private static void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            Debug.Print("TaskScheduler_UnobservedTaskException exception");

            Debug.Print(e.ToString());
        }

        private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Debug.Print("UnhandledExceptionEventArgs exception");

            Debug.Print(e.ToString());
        }
        private static void UnhandledDispatcherException(Object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            Debug.Print(e.ToString());

        }
        private static void AppExit(object sender, EventArgs e)
        {
            Debug.Print(e.ToString());

            HubManager.Instance.Logout();
            if (client.BaseAddress != null)
            {
             client.PostAsJsonAsync(client.BaseAddress + "api/logout", User.Instance.UserEntity);
            }
        }
     

        public static void InitAfterConnection()
        {
            mainMenu = new MainMenu();
            editeur = unityContainer.Resolve<Editeur>();
            quickPlayMenu = new QuickPlayMenu();
            tournementMenu = new TournementMenu();
            tournementTree = new TournementTree();
        }

        public static void InitializeUnityDependencyInjection()
        {
            unityContainer = new UnityContainer();

            // Managers
            unityContainer.RegisterType<GameManager>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<GameRequestManager>(new ContainerControlledLifetimeManager());

            //Hub instantiations
            unityContainer.RegisterType<IBaseHub, ChatHub>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<IBaseHub,GameWaitingRoomHub>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<IBaseHub, TournamentWaitingRoomHub>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<IBaseHub,GameHub>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<IBaseHub,FriendsHub>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<IBaseHub, EditionHub>(new ContainerControlledLifetimeManager());


            //View models instantiations
            unityContainer.RegisterType<MatchmakingViewModel>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<ChatListViewModel>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<ChatListItemViewModel>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<ChatViewModel>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<ChannelViewModel>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<JoinChannelListViewModel>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<JoinChannelViewModel>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<TournamentViewModel>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<AuthenticateViewModel>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<ConnectServerViewModel>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<SignupViewModel>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<HomeViewModel>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<EditorViewModel>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<FriendListViewModel>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<FriendListItemViewModel>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<UserProfileViewModel>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<StoreViewModel>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<AddUserViewModel>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<TutorialViewModel>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<MasterGameState>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<SlaveGameState>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<EditorUsersViewModel>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<CreateMapViewModel>(new ContainerControlledLifetimeManager());


            unityContainer.RegisterType<OnlineEditorState>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<OfflineEditorState>(new ContainerControlledLifetimeManager());

            unityContainer.RegisterType<SlaveGameState>(new ContainerControlledLifetimeManager());

            unityContainer.RegisterType<EditorViewModel>(new ContainerControlledLifetimeManager());


            //Rest services instantiations
            unityContainer.RegisterType<MapService>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<UserService>(new ContainerControlledLifetimeManager());

            //Other services
            unityContainer.RegisterType<MapManager>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<StoreService>(new ContainerControlledLifetimeManager());
        }

        static void ExecuterQuandInactif(object sender, EventArgs e)
        {
            FonctionsNatives.Message message;

            while (!FonctionsNatives.PeekMessage(out message, IntPtr.Zero, 0, 0, 0))
            {
                TimeSpan currentTime = chrono.Elapsed;
                TimeSpan elapsedTime = currentTime - dernierTemps;
                dernierTemps = currentTime;

                tempsAccumule += elapsedTime;

                if (tempsAccumule >= tempsEcouleVoulu)
                {
                    lock (unLock)
                    {
                        if (formManager != null && peutAfficher)
                            formManager.MettreAJour((double)tempsAccumule.Ticks / TimeSpan.TicksPerSecond);
                    }
                    tempsAccumule = TimeSpan.Zero;
                }
            }
        }

    }
    static partial class FonctionsNatives
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct Message
        {
            public IntPtr hWnd;
            public uint Msg;
            public IntPtr wParam;
            public IntPtr lParam;
            public uint Time;
            public System.Drawing.Point Point;
        }

        [DllImport("User32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PeekMessage(out Message message, IntPtr hWnd, uint filterMin, uint filterMax, uint flags);

        [DllImport(@"Noyau.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool executerTests();
    }




}
