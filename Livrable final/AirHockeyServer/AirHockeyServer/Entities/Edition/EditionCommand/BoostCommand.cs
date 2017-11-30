using AirHockeyServer.Entities.Messages.Edition;

namespace AirHockeyServer.Entities.EditionCommand
{
    public class BoostCommand : AbstractEditionCommand
    {
        public float[] Position { get; set; }
        public float Rotation { get; set; }
        public float[] Scale { get; set; }

        public BoostCommand(string objectUuid) : base(objectUuid)
        {
        }
    }
}