using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceGraphique.Entities.EditorCommand;

namespace InterfaceGraphique.Entities.EditonCommand
{
    class BoostCommand : AbstractEditionCommand
    {
        public float[] Position { get; set; }

        public BoostCommand(string objectUuid) : base(objectUuid)
        {
        }

        public override void ExecuteCommand()
        {
            FonctionsNatives.createBoost(objectUuid.ToCharArray(), Position);
        }
    }
}
