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
using InterfaceGraphique.CommunicationInterface;
using InterfaceGraphique.Menus;

namespace InterfaceGraphique
{
    static class Program {
        private const int NB_IMAGES_PAR_SECONDE = 120;

        public static Object unLock = new Object();
        public static bool peutAfficher = true;

        public static FormManager FormManager { get { return formManager; } }
        public static MainMenu MainMenu { get { return mainMenu; } }
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
        public static Chat Chat { get { return chat; } set { chat = value; } }


        private static FormManager formManager;
        private static MainMenu mainMenu;
        private static Editeur editeur;
        private static ConfigurationMenu configurationMenu;
        private static QuickPlay quickPlay;
        private static TestMode testMode;
        private static GeneralProperties generalProperties;
        private static QuickPlayMenu quickPlayMenu;
        private static TournementMenu tournementMenu;
        private static TournementTree tournementTree;
        private static CreditsMenu creditsMenu;
        private static Panel openGLPanel;
        private static Login login;
        private static Chat chat;
        private static TimeSpan dernierTemps;
        private static TimeSpan tempsAccumule;
        private static Stopwatch chrono = Stopwatch.StartNew();
        private static TimeSpan tempsEcouleVoulu = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / NB_IMAGES_PAR_SECONDE);


        public static HttpClient client = new HttpClient();

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
            ChatHub chatHub = new ChatHub();
            chatHub.EstablishConnection();
            //ChatConnection chatConnection = new ChatConnection();
            //chatConnection.EstablishConnection();
            //ChatMessage temp = new ChatMessage()
            //{
            //    MessageValue = "Allô chère madame",
            //    Recipient = "",
            //    Sender = "",
            //    TimeStamp = DateTime.Now
            //};
            //chatConnection.Send(temp);
            //ChatMessage temp2 = new ChatMessage()
            //{
            //    MessageValue = "foo bar ssss",
            //    Recipient = "",
            //    Sender = "",
            //    TimeStamp = DateTime.Now
            //};
            //chatConnection.Send(temp2);

            chrono.Start();
            Application.Idle += ExecuterQuandInactif;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Program.client = new HttpClient();
            Program.client.BaseAddress = new Uri("http://localhost:63056/");
            Program.client.DefaultRequestHeaders.Accept.Clear();
            Program.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            login = new Login();
            openGLPanel = new Panel();
            formManager = new FormManager();
            mainMenu = new MainMenu();
            editeur = new Editeur();
            configurationMenu = new ConfigurationMenu();
            quickPlay = new QuickPlay();
            testMode = new TestMode();
            generalProperties = new GeneralProperties();
            quickPlayMenu = new QuickPlayMenu();
            tournementMenu = new TournementMenu();
            tournementTree = new TournementTree();
            creditsMenu = new CreditsMenu();

            FonctionsNatives.loadSounds();

            formManager.CurrentForm = login;
            Application.Run(formManager);
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
