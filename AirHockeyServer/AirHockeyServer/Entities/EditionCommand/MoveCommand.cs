
using AirHockeyServer.Entities.Messages.Edition;

namespace AirHockeyServer.Entities.EditionCommand
{
    public class MoveCommand : AbstractEditionCommand
    {
        public float[] Position { get; set; }

        public MoveCommand(string uuid) : base(uuid)
        {
        }
    }
}