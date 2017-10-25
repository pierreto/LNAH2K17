using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceGraphique.Entities;
using Microsoft.AspNet.SignalR.Client;

namespace InterfaceGraphique.CommunicationInterface
{
    class EditionBrowserHub : IBaseHub
    {
        protected string username;
        protected IHubProxy hubProxy;
        public void InitializeHub(HubConnection connection, string username)
        {
            this.username = username;
            hubProxy = connection.CreateHubProxy("EditionBrowserHub");

        }

        public async Task<List<MapEntity>> getAvailableMap()
        {
            return await hubProxy.Invoke<List<MapEntity>>("GetAvailableMapList");
        }

        public void Logout()
        {
            hubProxy?.Invoke("Disconnect", this.username).Wait();
        }
    }
}
