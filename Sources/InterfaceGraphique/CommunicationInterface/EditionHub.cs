using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceGraphique.Entities;
using InterfaceGraphique.Entities.EditorCommand;
using Microsoft.AspNet.SignalR.Client;

namespace InterfaceGraphique.CommunicationInterface
{
    public class EditionHub : IBaseHub
    {
        public event Action<AbstractEditionCommand> NewCommand;

        protected string username;
        private IHubProxy hubProxy;

        private MapEntity map;

        public void InitializeHub(HubConnection connection, string username)
        {
            this.username = username;
            hubProxy = connection.CreateHubProxy("EditionHub");

        }

        public void JoinPublicRoom(MapEntity mapEntity)
        {
            this.map = mapEntity;
            mapEntity.Id = 1;
            hubProxy.Invoke("JoinPublicRoom", mapEntity);

            hubProxy.On<AbstractEditionCommand>("NewCommand", command =>
            {
                NewCommand?.Invoke(command);
            });
        }

        
        public void JoinPrivateRoom(MapEntity mapEntity, string password)
        {
            this.map = mapEntity;

            hubProxy.Invoke("JoinPrivateRoom", mapEntity, password);

        }

        public void SendEditorCommand(AbstractEditionCommand command)
        {
            hubProxy.Invoke("SendEditionCommand", this.map.Id, command);
        }



        public void LeaveRoom()
        {
            hubProxy.Invoke("LeaveRoom", this.map.Id);
        }



        public void Logout()
        {
            hubProxy?.Invoke("Disconnect", this.username).Wait();
            this.map = null;
        }
    }
}
