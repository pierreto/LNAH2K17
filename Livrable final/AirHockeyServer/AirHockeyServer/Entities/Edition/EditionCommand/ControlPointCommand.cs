using AirHockeyServer.Entities.Messages.Edition;

namespace AirHockeyServer.Entities.EditionCommand
{
    public class ControlPointCommand : AbstractEditionCommand
    {
        public string Username { get; set; }
        public float[] Position { get; set; }

        public ControlPointCommand(string objectUuid) : base(objectUuid)
        {
        }
    }
}