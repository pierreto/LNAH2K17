using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceGraphique.Entities.EditorCommand;

namespace InterfaceGraphique.Entities.Editor.EditonCommand
{
    class ControlPointCommand : AbstractEditionCommand
    {
        public string Username { get; set; }
        public float[] Position { get; set; }

        public ControlPointCommand(string objectUuid) : base(objectUuid)
        {
        }

        public override void ExecuteCommand()
        {
            FonctionsNatives.setControlPointPosition(Username, objectUuid, Position);
        }
    }
}
