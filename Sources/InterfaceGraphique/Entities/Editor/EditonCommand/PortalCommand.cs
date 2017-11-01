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
        public float[] EndPosition { get; set; }

        public PortalCommand(string objectUuid) : base(objectUuid)
        {
        }

        public override void ExecuteCommand()
        {
            FonctionsNatives.createPortal( objectUuid.ToCharArray(), StartPosition, EndUuid.ToCharArray(), EndPosition);  
        }
    }
}
