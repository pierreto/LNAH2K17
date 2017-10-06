using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.Practices.Unity;

namespace InterfaceGraphique.CommunicationInterface
{

    //CLASSE TRES TEMPORAIRE
    class HubManager
    {
        private HubConnection connection;

        private ChatHub chatHub;


        private List<IBaseHub> hubs;

        public async Task EstablishConnection(string serverIp, string username)
        {
            this.connection = new HubConnection("http://" + serverIp + ":63056/signalr");

            this.AddHubs();

            this.InitializeHubs(username);

            await this.connection.Start();
        }

        private void AddHubs()
        {
            this.hubs = new List<IBaseHub>
            {
                Program.unityContainer.Resolve<ChatHub>(),
                Program.unityContainer.Resolve<WaitingRoomHub>(),
                Program.unityContainer.Resolve<GameHub>()
            };
        }

        private void InitializeHubs(string username)
        {
            foreach (IBaseHub hub in this.hubs)
            {
                hub.InitializeHub(this.connection, username);
            }
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
