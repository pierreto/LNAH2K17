using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceGraphique.Entities.EditorCommand;

namespace InterfaceGraphique.Entities.EditonCommand
{
    class PortalCommand : AbstractEditionCommand
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

        public override void ExecuteCommand()
        {
            Program.Editeur.ExecuteCommandOnMainThread(CreatePortal);
        }

        public void CreatePortal()
        {
            FonctionsNatives.createPortal( objectUuid, StartPosition, StartRotation,StartScale, EndUuid, EndPosition, EndRotation, EndScale);
        }
    }
}
