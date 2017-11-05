using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceGraphique.Entities.EditorCommand;

namespace InterfaceGraphique.Entities.EditonCommand
{
    class TransformCommand : AbstractEditionCommand
    {
        public string Username { get; set; }
        public float[] TransformMatrix { get; set; }



        public TransformCommand(string uuid ) : base(uuid)
        {
        }

        public override void ExecuteCommand()
        {
            FonctionsNatives.setTransformByUUID(Username, objectUuid, TransformMatrix);
        }
    }
}
