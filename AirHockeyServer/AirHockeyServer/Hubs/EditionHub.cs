using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AirHockeyServer.Entities;
using AirHockeyServer.Entities.Edition.EditionCommand;
using AirHockeyServer.Entities.EditionCommand;
using AirHockeyServer.Entities.Messages;
using AirHockeyServer.Entities.Messages.Edition;
using AirHockeyServer.Services;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AirHockeyServer.Hubs
{
    public class EditionHub : BaseHub
    {
        private EditionService editionService;
        private UserService userService;
        private JsonSerializerSettings serializer;

        public EditionHub(EditionService editionService, ConnectionMapper connectionMapper, UserService userService)
            : base(connectionMapper)
        {
            this.editionService = editionService;
            this.serializer = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects
                
            };
            this.userService = userService;

        }

        public async Task<List<OnlineUser>> JoinPublicRoom(string username, MapEntity map)
        {
            return await DefaultJoin(username, map);
        }

        private async Task<List<OnlineUser>> DefaultJoin(string username, MapEntity map)
        {
            string mapGroupId = ObtainEditionGroupIdentifier((int)map.Id);

            EditionGroup editionGroup;
            if (!editionService.UsersPerGame.ContainsKey(mapGroupId))
            {
                editionGroup = new EditionGroup();
                editionService.UsersPerGame.Add(mapGroupId, editionGroup);
            }
            else
            {
                editionGroup = editionService.UsersPerGame[mapGroupId];
            }
            UserEntity user = await this.userService.GetUserByUsername(username);
            
            OnlineUser newUser = new OnlineUser()
            {
                Username = username,
                HexColor = editionGroup.lockNextColor(),
                ProfilePicture = user.Profile

            };
            //This shouldnt be managed here... He should be managed at the login point at least
            //But i have no choice for now
            ConnectionMapper.AddUserConnection(Context.ConnectionId, newUser);

            editionGroup.AddUser(newUser);

            //Add the connection id to the map group
            await Groups.Add(Context.ConnectionId, mapGroupId);

            //Broadcast to others users of the group that a new user arrived 
            Clients.Group(mapGroupId, Context.ConnectionId).NewUser(newUser);

            return editionGroup.users;

        }
        public async Task<List<OnlineUser>> JoinPrivateRoom(string username, MapEntity map, string password)
        {
            //Password validation
            return await DefaultJoin(username, map);
        }

        public void SendEditionCommand(int mapId, string editionCommand)
        {
            Clients.Group(ObtainEditionGroupIdentifier(mapId), Context.ConnectionId).NewCommand(editionCommand);

            //Clients.Group(ObtainEditionGroupIdentifier(mapId)).NewCommand(editionCommand);
        }

        public void SendSelectionCommand(int mapId, SelectionCommand selection)
        {
            //We update the current selection node list selected by the user
            OnlineUser user = ConnectionMapper.GetUserFromConnectionId(Context.ConnectionId);



            if (selection.DeselectAll)
            {
                user.UuidsSelected.Clear();
            }
            else
            {
                
                if (selection.IsSelected)
                {
                    user.UuidsSelected.Add(selection.ObjectUuid);
                }
                else
                {
                    user.UuidsSelected.Remove(selection.ObjectUuid);
                }
                
            }
            string str = JsonConvert.SerializeObject(selection, serializer);

            JObject jsonObject = JObject.Parse(str);
            jsonObject["$type"] = "InterfaceGraphique.Entities.EditonCommand.SelectionCommand, InterfaceGraphique";
            Clients.Group(ObtainEditionGroupIdentifier(mapId), Context.ConnectionId).NewCommand(jsonObject.ToString(Formatting.None, null));

        }

        public async Task LeaveRoom(int gameId)
        {
            await Groups.Remove(Context.ConnectionId, ObtainEditionGroupIdentifier(gameId) );

            OnlineUser userThatLeft = ConnectionMapper.GetUserFromConnectionId(Context.ConnectionId);
            editionService.UsersPerGame[ObtainEditionGroupIdentifier(gameId)].RemoveUser(userThatLeft);
            Clients.Group(ObtainEditionGroupIdentifier(gameId), Context.ConnectionId).UserLeaved(userThatLeft.Username);

            //For now we do this, but the should be done in another 
            //ConnectionMapper.RemoveUserConnection(Context.ConnectionId);
        }

        public static string ObtainEditionGroupIdentifier(int id)
        {
            return "EditionHub" + id;
        }

        public async Task Disconnect(int gameId)
        {
            await LeaveRoom(gameId);
            base.Disconnect();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            // TODO ANY SPECIAL ACTION?
            
            return base.OnDisconnected(stopCalled);
        }
    }
}