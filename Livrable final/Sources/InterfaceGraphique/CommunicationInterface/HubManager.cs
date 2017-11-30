using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.Practices.Unity;
using InterfaceGraphique.CommunicationInterface.WaitingRooms;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using InterfaceGraphique.Controls.WPF.Home;
using InterfaceGraphique.Controls.WPF.MainMenu;
using InterfaceGraphique.Controls.WPF.Chat;

namespace InterfaceGraphique.CommunicationInterface
{
    public class HubManager
    {
        private static HubManager instance;
        public string IpAddress { get; set; }

        private HubConnection connection;
        public HubConnection Connection
        {
            get;
            set;
        }

        private List<IBaseHub> hubs;
        public static HubManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new HubManager();
                }
                return instance;
            }
        }

        private HubManager()
        {
        }

        public async Task EstablishConnection(string serverIp)
        {
            this.connection = new HubConnection("http://" + serverIp + ":63056/signalr");
            //connection.TraceLevel = TraceLevels.All;
            //connection.TraceWriter = Console.Out;

            this.AddHubs();

            this.InitializeHubs();

            this.connection.Reconnecting += ConnectionClosed;

            await this.connection.Start();

            IpAddress = serverIp;
        }

        private void ConnectionClosed()
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 3000;
            timer.Elapsed += (timerSender, e) => EndOfTimer(timer);

            timer.Start();
        }

        public void EndOfTimer(System.Timers.Timer timer)
        {
            timer.Stop();

            if (this.connection.State == ConnectionState.Reconnecting)
            {
                HandleDisconnection();
            }

        }

        public static void HandleDisconnection()
        {
            System.Windows.Forms.MessageBox.Show(
                 @"Le lien entre vous et le serveur s'est brisé. Vérifiez votre connection internet. Sinon ce peut être dû à une catastrophe naturelle, des chargés de laboratoires ou autre",
                 @"Catastrophe",
                 MessageBoxButtons.OK, MessageBoxIcon.Error);
            // await LeaveHubs();
            Program.FormManager.Invoke(new MethodInvoker(() =>
            {
                Program.FormManager.CurrentForm = Program.HomeMenu;
                Program.unityContainer.Resolve<ChatViewModel>().UndockedChat?.Close();
                Program.unityContainer.Resolve<MainMenuViewModel>().OnlineMode = false;
                Program.HomeMenu.ChangeViewTo(Program.unityContainer.Resolve<HomeViewModel>());
                Program.InitializeUnityDependencyInjection();
                Program.unityContainer.Resolve<GameWaitingRoomHub>().OnDisconnect();
                Program.unityContainer.Resolve<EditionHub>().OnDisconnect();
                Program.Restart();
            }));
        }

        public void AddHubs()
        {
            this.hubs = new List<IBaseHub>
            {
                Program.unityContainer.Resolve<ChatHub>(),
                Program.unityContainer.Resolve<TournamentWaitingRoomHub>(),
                Program.unityContainer.Resolve<GameWaitingRoomHub>(),
                Program.unityContainer.Resolve<GameHub>(),
                Program.unityContainer.Resolve<EditionHub>(),
                Program.unityContainer.Resolve<FriendsHub>()
            };
        }

        public void InitializeHubs()
        {
            foreach (IBaseHub hub in this.hubs)
            {
                hub.InitializeHub(this.connection);
            }
        }

        public async Task LeaveHubs()
        {
            foreach (IBaseHub hub in this.hubs)
            {
                await hub.LeaveRoom();
            }
        }

        public async Task Logout()
        {
            if (this.hubs != null)
            {
                foreach (IBaseHub hub in this.hubs)
                {
                    await hub.Logout();
                }
                this.connection.Stop();
            }

        }

    }
}
