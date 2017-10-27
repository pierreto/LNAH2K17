using AirHockeyServer.Entities.Messages.Edition;

namespace AirHockeyServer.Entities.EditionCommand
{
    public class PortalCommand : AbstractEditionCommand
    {
        public float[] StartPosition { get; set; }
        public float[] EndPosition { get; set; }

        public PortalCommand(string objectUuid) : base(objectUuid)
        {
        }
    }
}