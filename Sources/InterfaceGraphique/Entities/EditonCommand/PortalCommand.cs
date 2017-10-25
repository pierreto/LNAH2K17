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
        public int[] Position { get; set; }

        public PortalCommand(string objectUuid) : base(objectUuid)
        {
        }

        public override void ExecuteCommand()
        {
            FonctionsNatives.changerModeleEtat((int)MODELE_ETAT.CREATION_PORTAIL);  
        }
    }
}
