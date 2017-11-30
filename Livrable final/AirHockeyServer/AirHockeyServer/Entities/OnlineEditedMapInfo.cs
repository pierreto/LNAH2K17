namespace AirHockeyServer.Entities
{
    public class OnlineEditedMapInfo
    {
        private bool isPublic;
        private string editionGroupId;
        private string mapName;
        private int numberOfPlayer;
        private int maxNumberOfPlayer;


        public string OnlineGroupId
        {
            get => editionGroupId;
            set => editionGroupId = value;
        }

        public string MapName
        {
            get => mapName;
            set => mapName = value;
        }

        public int NumberOfPlayer
        {
            get => numberOfPlayer;
            set => numberOfPlayer = value;
        }

        public int MaxNumberOfPlayer
        {
            get => maxNumberOfPlayer;
            set => maxNumberOfPlayer = value;
        }

        public bool IsPublic
        {
            get => isPublic;
            set => isPublic = value;
        }
    }
}