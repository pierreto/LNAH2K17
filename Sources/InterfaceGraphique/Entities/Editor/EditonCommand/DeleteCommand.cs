using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceGraphique.Entities.EditorCommand;

namespace InterfaceGraphique.Entities.Editor.EditonCommand
{
    class DeleteCommand : AbstractEditionCommand
    {
        public string Username { get; set; }
        public DeleteCommand(string objectUuid) : base(objectUuid)
        {
        }

        public override void ExecuteCommand()
        {
            Program.Editeur.ExecuteCommandOnMainThread(Delete);
        }

        public void Delete()
        {
            FonctionsNatives.deleteNode(Username, objectUuid);
        }
    }
}
