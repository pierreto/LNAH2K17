namespace AirHockeyServer.Entities.Messages
{
    public class RoomConnectionMessage
    {
        private string roomUuid;

        public RoomConnectionMessage(string roomRoomUuid)
        {
            this.roomUuid = roomRoomUuid;
        }

        public string RoomUuid
        {
            get => roomUuid;
            set => roomUuid = value;
        }
    }
}