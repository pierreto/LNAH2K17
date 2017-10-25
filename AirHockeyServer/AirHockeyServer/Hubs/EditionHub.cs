using AirHockeyServer.Entities;
using AirHockeyServer.Entities.EditionCommand;
using AirHockeyServer.Entities.Messages;
using AirHockeyServer.Entities.Messages.Edition;
using Microsoft.AspNet.SignalR;

namespace AirHockeyServer.Hubs
{
    public class EditionHub : Hub
    {

        public void JoinPublicRoom(MapEntity map)
        {
            Groups.Add(Context.ConnectionId, ObtainEditionGroupIdentifier(map.Id));
        }
        public void JoinPrivateRoom(MapEntity map, string password)
        {
            //Password validation



            Groups.Add(Context.ConnectionId, ObtainEditionGroupIdentifier(map.Id));
        }

        public void SendEditionCommand(int mapId, AbstractEditionCommand editionCommand)
        {
            Clients.Group(ObtainEditionGroupIdentifier(mapId)).NewCommand(editionCommand);
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