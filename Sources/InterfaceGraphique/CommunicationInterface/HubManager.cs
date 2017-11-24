using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.Practices.Unity;
using InterfaceGraphique.CommunicationInterface.WaitingRooms;
using System.Threading;

namespace InterfaceGraphique.CommunicationInterface
{

    //CLASSE TRES TEMPORAIRE
    public class HubManager
    {
        private static HubManager instance;
        public string IpAddress { get; set; }
        private HubConnection connection;
        public HubConnection Connection { get; set; }

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

            this.AddHubs();

            this.InitializeHubs();

            await this.connection.Start();

            IpAddress = serverIp;
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

        public void LeaveHubs()
        {
            foreach(IBaseHub hub in this.hubs)
            {
                hub.LeaveRoom();
            }
        }
        public void LeaveEditorAndGameHubs()
        {
            foreach (IBaseHub hub in this.hubs)
            {
                if (hub is EditionHub || hub is GameHub || hub is GameWaitingRoomHub)
                {
                    hub.LeaveRoom();
                }
            }
        }

        public void Logout()
        {
            if (this.hubs != null)
            {
                foreach (IBaseHub hub in this.hubs)
                {
                    hub.Logout();
                }
                this.connection.Stop();
            }
    
        }
        
    }
}
