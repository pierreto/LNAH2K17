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
        public float[] Position { get; set; }
        public float Rotation { get; set; }
        public float[] Scale { get; set; }




        public TransformCommand(string uuid ) : base(uuid)
        {
        }

        public override void ExecuteCommand()
        {
            FonctionsNatives.setTransformByUUID(Username, objectUuid, Position,Rotation,Scale);
        }
    }
}
