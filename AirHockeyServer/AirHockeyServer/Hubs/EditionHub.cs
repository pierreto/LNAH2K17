using System.Collections.Generic;
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
        private Dictionary<string, List<OnlineUser>> users;

        public List<OnlineUser> JoinPublicRoom(string username, MapEntity map)
        {
            Groups.Add(Context.ConnectionId, ObtainEditionGroupIdentifier((int) map.Id));
            Clients.Group(ObtainEditionGroupIdentifier((int) map.Id), Context.ConnectionId).NewUser(username, "#ff0000");
            return users[ObtainEditionGroupIdentifier((int) map.Id)];
        }
        public void JoinPrivateRoom(string username, MapEntity map, string password)
        {
            //Password validation


            Groups.Add(Context.ConnectionId, ObtainEditionGroupIdentifier((int)map.Id));
        }

        public void SendEditionCommand(int mapId, string editionCommand)
        {
            Clients.Group(ObtainEditionGroupIdentifier(mapId), Context.ConnectionId).NewCommand(editionCommand);

            //Clients.Group(ObtainEditionGroupIdentifier(mapId)).NewCommand(editionCommand);
        }

        public void LeaveRoom(int gameId)
        {
            Groups.Remove(Context.ConnectionId, this.GetType().Name + gameId);
        }

        private string ObtainEditionGroupIdentifier(int id)
        {
            return this.GetType().Name + id;
        }
    }
}