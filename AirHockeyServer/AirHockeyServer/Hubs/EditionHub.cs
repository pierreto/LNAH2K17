using System.Collections.Generic;
using System.Threading.Tasks;
using AirHockeyServer.Entities;
using AirHockeyServer.Entities.EditionCommand;
using AirHockeyServer.Entities.Messages;
using AirHockeyServer.Entities.Messages.Edition;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;

namespace AirHockeyServer.Hubs
{
    public class EditionHub : Hub
    {
        private EditionService editionService;
        public EditionHub(EditionService editionService)
        {
            this.editionService = editionService;
        }

        public async Task<List<OnlineUser>> JoinPublicRoom(string username, MapEntity map)
        {

            string mapGroupId = ObtainEditionGroupIdentifier((int) map.Id);

   
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
            //Clients.Group(mapGroupId).NewUser(newUser);

            return editionService.UsersPerGame[ObtainEditionGroupIdentifier((int) map.Id)];
        }
        public void JoinPrivateRoom(string username, MapEntity map, string password)
        {
            //Password validation


            Groups.Add(Context.ConnectionId, ObtainEditionGroupIdentifier((int)map.Id));
        }

        public void SendEditionCommand(int mapId, string editionCommand)
        {
            //Clients.Group(ObtainEditionGroupIdentifier(mapId), Context.ConnectionId).NewCommand(editionCommand);

            Clients.Group(ObtainEditionGroupIdentifier(mapId)).NewCommand(editionCommand);
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