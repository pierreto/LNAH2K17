using AirHockeyServer.Entities.Messages.Edition;

namespace AirHockeyServer.Entities.EditionCommand
{
    public class PortalCommand : AbstractEditionCommand
    {
        public int[] Position { get; set; }

        public PortalCommand(string objectUuid) : base(objectUuid)
        {
        }
    }
}