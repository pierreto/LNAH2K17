
using AirHockeyServer.Entities.Messages.Edition;

namespace AirHockeyServer.Entities.EditionCommand
{
    public class MoveCommand : AbstractEditionCommand
    {
        public string Username { get; set; }
        public float[] Position { get; set; }
        public float Rotation { get; set; }
        public float[] Scale { get; set; }

        public MoveCommand(string uuid) : base(uuid)
        {
        }
    }
}