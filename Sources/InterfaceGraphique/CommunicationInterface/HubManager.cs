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

            ChatHub chatHub = Program.unityContainer.Resolve<ChatHub>();

            this.hubs = new List<IBaseHub>();
            this.hubs.Add(chatHub);
            this.InitializeHubs(connection,username);



            await this.connection.Start();
        }

        private void InitializeHubs(HubConnection connection,string username)
        {
            foreach (IBaseHub hub in this.hubs)
            {
                hub.InitializeHub(connection, username);
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
