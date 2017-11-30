using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceGraphique.Entities.EditorCommand
{
    public abstract class AbstractEditionCommand
    {
        protected string objectUuid;

        protected AbstractEditionCommand(string objectUuid)
        {
            this.objectUuid = objectUuid;
        }
        public string ObjectUuid
        {
            get => objectUuid;
            set => objectUuid = value;
        }

        public void ExecuteCommandOnMainThread()
        {
            Program.Editeur.ExecuteCommandOnMainThread(ExecuteCommand);
        }
        public abstract void ExecuteCommand();
    }
}
