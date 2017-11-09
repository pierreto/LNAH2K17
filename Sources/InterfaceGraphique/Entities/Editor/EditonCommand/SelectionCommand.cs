using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceGraphique.Entities.EditorCommand;

namespace InterfaceGraphique.Entities.EditonCommand
{
    public class SelectionCommand : AbstractEditionCommand
    {
        public string Username { get; set; }
        public bool IsSelected { get; set; }
        public bool DeselectAll { get; set; }

        public SelectionCommand(string objectUuid) : base(objectUuid)
        {
        }

        public override void ExecuteCommand()
        {
            FonctionsNatives.setElementSelection(Username, objectUuid, IsSelected, DeselectAll);
        }
    }
}
