namespace AirHockeyServer.Entities.Messages.Edition
{
    public class WallCommand : AbstractEditionCommand
    {
        private float[] startPosition; //DEPRECATED TO REMOVE
        private float[] endPosition; //DEPRECATED TO REMOVE

        public float[] Position { get; set; }
        public float Rotation { get; set; }
        public float[] Scale { get; set; }

        public WallCommand(string objectUuid) : base(objectUuid)
        {
        }

    }
}