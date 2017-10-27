using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Net.WebSockets;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.CommunicationInterface.RestInterface;
using InterfaceGraphique.Controls;
using InterfaceGraphique.Controls.WPF;
using InterfaceGraphique.Controls.WPF.Chat;
using InterfaceGraphique.Controls.WPF.Editor;
using InterfaceGraphique.Entities;
using InterfaceGraphique.Menus;
using Microsoft.AspNet.SignalR.Client;
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

namespace InterfaceGraphique
{
    static class Program {
        private const int NB_IMAGES_PAR_SECONDE = 120;

        public static Object unLock = new Object();
        public static bool peutAfficher = true;

        public static FormManager FormManager { get { return formManager; } }
        public static MainMenu MainMenu { get { return mainMenu; } }
        public static HomeMenu HomeMenu { get { return homeMenu; } }
        //public static Login Login {  get { return login; } }
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
        public static UserEntity user;
        public static LobbyHost LobbyHost { get { return lobbyHost; } set { lobbyHost = value; } }
        public static OnlineTournament OnlineTournament { get { return onlineTournament;  } set { onlineTournament = value; } }
        public static EditorHost EditorHost { get { return editorHost; } set { editorHost = value; } }


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
            WPFApplication.Start();

            InitializeUnityDependencyInjection();

            //login = unityContainer.Resolve<Login>(); ;
            openGLPanel = new Panel();
            formManager = new FormManager();
            homeMenu = new HomeMenu();
            //mainMenu = new MainMenu();
            editeur = unityContainer.Resolve<Editeur>();
            configurationMenu = new ConfigurationMenu();
            quickPlay = new QuickPlay();
            testMode = new TestMode();
            generalProperties = new GeneralProperties();
            //quickPlayMenu = new QuickPlayMenu();
            tournementMenu = new TournementMenu();
            tournementTree = new TournementTree();
            creditsMenu = new CreditsMenu();
            lobbyHost = new LobbyHost();
            onlineTournament = new OnlineTournament();
            editorHost = new EditorHost();

            FonctionsNatives.loadSounds();

            formManager.CurrentForm = homeMenu;
            // formManager.CurrentForm = login;
            Application.Run(formManager);

        }
        

        public static void InitAfterConnection()
        {
            mainMenu = new MainMenu();
            quickPlayMenu = new QuickPlayMenu();
        }

        private static void InitializeUnityDependencyInjection()
        {
            unityContainer = new UnityContainer();

            //Hub instantiations
            unityContainer.RegisterType<IBaseHub, ChatHub>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<IBaseHub,GameWaitingRoomHub>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<IBaseHub, TournamentWaitingRoomHub>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<IBaseHub,GameHub>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<IBaseHub, EditionHub>(new ContainerControlledLifetimeManager());


            //View models instantiations
            unityContainer.RegisterType<MatchmakingViewModel>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<ChatViewModel>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<TournamentViewModel>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<AuthenticateViewModel>();
            unityContainer.RegisterType<SignupViewModel>();
            unityContainer.RegisterType<HomeViewModel>();
            unityContainer.RegisterType<ConnectServerViewModel>(); 
            unityContainer.RegisterType<EditorViewModel>(new ContainerControlledLifetimeManager());


            //Rest services instantiations
            unityContainer.RegisterType<MapService>();


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
