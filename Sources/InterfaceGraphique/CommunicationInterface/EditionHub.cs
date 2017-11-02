using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceGraphique.Entities;
using InterfaceGraphique.Entities.Editor;
using InterfaceGraphique.Entities.EditorCommand;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;

namespace InterfaceGraphique.CommunicationInterface
{
    public class EditionHub : IBaseHub
    {
        public event Action<AbstractEditionCommand> NewCommand;
        public event Action<OnlineUser> NewUser;
        public event Action<string> UserLeft;


        protected string username;
        private IHubProxy hubProxy;

        private MapEntity map;

        public void InitializeHub(HubConnection connection)
        {
            hubProxy = connection.CreateHubProxy("EditionHub");

        }

        public async Task<List<OnlineUser>> JoinPublicRoom(MapEntity mapEntity)
        {
            InializeEvents();
            this.map = mapEntity;
            return await hubProxy.Invoke<List<OnlineUser>>("JoinPublicRoom", User.Instance.UserEntity.Username, mapEntity);


     
        }

        private void InializeEvents()
        {
            hubProxy.On<string>("NewCommand", command =>
            {
                var rcmd = JsonConvert.DeserializeObject<AbstractEditionCommand>(
                    command,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });
                NewCommand?.Invoke(rcmd);
            });

            hubProxy.On<OnlineUser>("NewUser", user =>
            {
                NewUser?.Invoke(user);

            });
            hubProxy.On<string>("UserLeaved", username =>
            {
                UserLeft?.Invoke(username);

            });
        }
        
        public void JoinPrivateRoom(MapEntity mapEntity, string password)
        {
            this.map = mapEntity;

            hubProxy.Invoke("JoinPrivateRoom", mapEntity, password);

        }

        public void SendEditorCommand(AbstractEditionCommand command)
        {
            var serializer = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            var str = JsonConvert.SerializeObject(command, serializer);
            hubProxy.Invoke("SendEditionCommand", this.map.Id, str);
        }



        public async Task LeaveRoom()
        {
            if (map != null)
            {
                await hubProxy.Invoke("LeaveRoom", this.map.Id);
            }
        }


        public void Logout()
        {
           /* hubProxy?.Invoke("Disconnect", this.username).Wait();
            this.map = null;*/
        }
    }
}
