using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceGraphique.Entities;
using InterfaceGraphique.Entities.EditonCommand;
using InterfaceGraphique.Entities.Editor;
using InterfaceGraphique.Entities.EditorCommand;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;

namespace InterfaceGraphique.CommunicationInterface
{
    public class EditionHub : BaseHub, IBaseHub
    {
        public event Action<AbstractEditionCommand> NewCommand;
        public event Action<OnlineUser> NewUser;
        public event Action<string> UserLeft;


        protected string username;
        private IHubProxy hubProxy;
        private JsonSerializerSettings serializer;

        private MapEntity map;

        public EditionHub()
        {
            serializer = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
        }

        public void InitializeHub(HubConnection connection)
        {
            hubProxy = connection.CreateHubProxy("EditionHub");
            InializeEvents();
        }

        public async Task<List<OnlineUser>> JoinPublicRoom(MapEntity mapEntity)
        {
            this.map = mapEntity;
            try
            {
                return await hubProxy.Invoke<List<OnlineUser>>("JoinPublicRoom", User.Instance.UserEntity.Username, mapEntity);
            }
            catch (Exception e)
            {
                HandleError();
            }

            return null;
        }

        private void InializeEvents()
        {
            hubProxy.On<string>("NewCommand", command =>
            {
                AbstractEditionCommand rcmd = JsonConvert.DeserializeObject<AbstractEditionCommand>(
                    command,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                rcmd.ExecuteCommand();
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

            try
            {
                hubProxy.Invoke("JoinPrivateRoom", mapEntity, password);
            }
            catch (Exception e)
            {
                HandleError();
            }

        }

        public void SendEditorCommand(AbstractEditionCommand command)
        {
            var str = JsonConvert.SerializeObject(command, serializer);
            try
            {
                hubProxy.Invoke("SendEditionCommand", this.map.Id, str);
            }
            catch (Exception e)
            {
                HandleError();
            }
        }

        public void SendSelectionCommand(SelectionCommand command)
        {
            try
            {
                hubProxy.Invoke("SendSelectionCommand", this.map.Id, command);
            }
            catch (Exception e)
            {
                HandleError();
            }
        }



        public async Task LeaveRoom()
        {
            if (map != null)
            {
                Editeur.mapManager.SaveIcon();
                try
                {
                    await hubProxy.Invoke("LeaveRoom", this.map.Id);
                }
                catch (Exception e)
                {
                    HandleError();
                }
                Program.Editeur.LeaveOnlineEdition();
            }
        }


        public async Task Logout()
        {
            if (map != null)
            {
                try
                {
                    await hubProxy.Invoke("LeaveRoom", this.map.Id);
                }
                catch (Exception e)
                {
                    HandleError();
                }
                Program.Editeur.LeaveOnlineEdition();
            }
        }

        public void OnDisconnect()
        {
            //TODO
        }
    }
}
