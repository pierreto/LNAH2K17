using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AirHockeyServer.Entities;
using AirHockeyServer.Entities.Edition.EditionCommand;
using AirHockeyServer.Entities.EditionCommand;
using AirHockeyServer.Entities.Messages;
using AirHockeyServer.Entities.Messages.Edition;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AirHockeyServer.Hubs
{
    public class EditionHub : Hub
    {
        private EditionService editionService;
        private JsonSerializerSettings serializer;

        public EditionHub(EditionService editionService)
        {
            this.editionService = editionService;
            this.serializer = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects             };

        }

        public async Task<List<OnlineUser>> JoinPublicRoom(string username, MapEntity map)
        {
            return await DefaultJoin(username, map);
        }

        private async Task<List<OnlineUser>> DefaultJoin(string username, MapEntity map)
        {
            string mapGroupId = ObtainEditionGroupIdentifier((int)map.Id);


            if (!editionService.UsersPerGame.ContainsKey(mapGroupId))
            {
                List<OnlineUser> newEditionGroup = new List<OnlineUser>();
                editionService.UsersPerGame.Add(mapGroupId, newEditionGroup);
            }
            OnlineUser newUser = new OnlineUser()
            {
                Username = username,
                HexColor = this.editionService.Colors[editionService.UsersPerGame[mapGroupId].Count]
            };
            //This shouldnt be managed here... He should be managed at the login point at least
            //But i have no choice for now
            ConnectionMapper.AddUserConnection(Context.ConnectionId, newUser);

            editionService.UsersPerGame[mapGroupId].Add(newUser);

            //Add the connection id to the map group
            await Groups.Add(Context.ConnectionId, mapGroupId);

            //Broadcast to others users of the group that a new user arrived 
            Clients.Group(mapGroupId, Context.ConnectionId).NewUser(newUser);

            return editionService.UsersPerGame[ObtainEditionGroupIdentifier((int)map.Id)];

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
            editionService.UsersPerGame[ObtainEditionGroupIdentifier(gameId)].Remove(userThatLeft);
            Clients.Group(ObtainEditionGroupIdentifier(gameId), Context.ConnectionId).UserLeaved(userThatLeft.Username);

            //For now we do this, but the should be done in another 
            //ConnectionMapper.RemoveUserConnection(Context.ConnectionId);
        }

        public static string ObtainEditionGroupIdentifier(int id)
        {
            return "EditionHub" + id;
        }
    }
}