using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceGraphique.Entities.EditorCommand;

namespace InterfaceGraphique.Entities.EditonCommand
{
    class SelectionCommand : AbstractEditionCommand
    {
        public SelectionCommand(string objectUuid) : base(objectUuid)
        {
        }

        public override void ExecuteCommand()
        {
            FonctionsNatives.setElementAsSelected(objectUuid.ToCharArray());
        }
    }
}
