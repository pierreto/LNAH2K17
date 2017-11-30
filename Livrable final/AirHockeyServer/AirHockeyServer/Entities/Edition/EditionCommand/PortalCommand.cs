using AirHockeyServer.Entities.Messages.Edition;

namespace AirHockeyServer.Entities.EditionCommand
{
    public class PortalCommand : AbstractEditionCommand
    {
        public string EndUuid { get; set; }

        public float[] StartPosition { get; set; }
        public float StartRotation { get; set; }
        public float[] StartScale { get; set; }


        public float[] EndPosition { get; set; }
        public float EndRotation { get; set; }
        public float[] EndScale { get; set; }

        public PortalCommand(string objectUuid) : base(objectUuid)
        {
        }
    }
}