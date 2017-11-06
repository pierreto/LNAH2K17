using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceGraphique.Entities.EditorCommand;

namespace InterfaceGraphique.Entities.EditonCommand
{
    class WallCommand : AbstractEditionCommand
    {
        private float[] startPosition; //DEPRECATED TO REMOVE
        private float[] endPosition; //DEPRECATED TO REMOVE

        public float[] Position { get; set; }
        public float Rotation { get; set; }
        public float[] Scale { get; set; }

        public WallCommand(string objectUuid) : base(objectUuid)
        {
        }

        public override void ExecuteCommand()
        {
            FonctionsNatives.createWall( objectUuid, Position, Rotation, Scale);
        }
    }
}
