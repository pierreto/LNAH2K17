using AirHockeyServer.Entities.Messages;
using AirHockeyServer.Entities.Messages.Edition;
using Microsoft.AspNet.SignalR;

namespace AirHockeyServer.Hubs
{
    public class EditionHub : Hub, IRoomHub
    {

        public void JoinRoom(RoomConnectionMessage joinMessage )
        {
            Groups.Add(Context.ConnectionId, joinMessage.RoomUuid);
        }

        public void SendWall(WallMessage wallMessage)
        {
        
        }



        public void LeaveRoom(RoomConnectionMessage joinMessage)
        {
            Groups.Remove(Context.ConnectionId, joinMessage.RoomUuid);
        }
    }
}