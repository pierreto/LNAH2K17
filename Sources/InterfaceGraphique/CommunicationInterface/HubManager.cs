using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.Practices.Unity;
using System.Threading;

namespace InterfaceGraphique.CommunicationInterface
{

    //CLASSE TRES TEMPORAIRE
    public class HubManager
    {

        private static HubManager instance;

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

            //    this.InitializeHubs(username);
        }

        //public async Task EstablishConnection(string serverIp, string username)
        //{
        //    this.connection = new HubConnection("http://" + serverIp + ":63056/signalr");

        //    this.AddHubs();

        //    this.InitializeHubs(username);

        //    await this.connection.Start();
        //}

        public void AddHubs()
        {
            this.hubs = new List<IBaseHub>
            {
                Program.unityContainer.Resolve<ChatHub>(),
                Program.unityContainer.Resolve<WaitingRoomHub>(),
                Program.unityContainer.Resolve<GameHub>()
            };
        }

        public async Task InitializeHubs(string username)
        {
            foreach (IBaseHub hub in this.hubs)
            {
                hub.InitializeHub(this.connection, username);
            }
            await this.connection.Start();

        }

        public void Logout()
        {
            foreach (IBaseHub hub in this.hubs)
            {
                hub.Logout();
            }
            this.connection.Stop();
        }
    }
}
