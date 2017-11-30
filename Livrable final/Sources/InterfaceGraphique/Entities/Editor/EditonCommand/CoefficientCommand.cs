using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceGraphique.Entities.EditorCommand;

namespace InterfaceGraphique.Entities.Editor.EditonCommand
{
    class CoefficientCommand : AbstractEditionCommand
    {
        public float frictionCoeff { get; set; }

        public float reboundCoeff { get; set; }
        
        public float accCoeff { get; set; }

        public CoefficientCommand() : base("")
        {
        }

        public override void ExecuteCommand()
        {
            Program.GeneralProperties.SetCoefficientValues(new float[]{frictionCoeff, reboundCoeff, accCoeff});
        }
    }
}
