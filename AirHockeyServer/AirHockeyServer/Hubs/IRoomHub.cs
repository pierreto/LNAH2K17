using AirHockeyServer.Entities.Messages;

namespace AirHockeyServer.Hubs
{
    interface IRoomHub
    {
        void JoinRoom(RoomConnectionMessage roomConnectionMessage);
        void LeaveRoom(RoomConnectionMessage roomConnectionMessage);
    }
}
