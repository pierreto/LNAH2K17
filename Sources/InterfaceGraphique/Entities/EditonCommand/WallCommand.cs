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
        private float[] startPosition;
        private float[] endPosition;

        public WallCommand(string objectUuid) : base(objectUuid)
        {
        }

        public float[] StartPosition
        {
            get => startPosition;
            set => startPosition = value;
        }

        public float[] EndPosition
        {
            get => endPosition;
            set => endPosition = value;
        }


        public override void ExecuteCommand()
        {
            FonctionsNatives.createWall( objectUuid.ToCharArray(), StartPosition, EndPosition);
        }
    }
}
