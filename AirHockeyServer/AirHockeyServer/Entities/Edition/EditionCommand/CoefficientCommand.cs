using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirHockeyServer.Entities.Messages.Edition;

namespace AirHockeyServer.Entities.Edition.EditionCommand
{
    public class CoefficientCommand : AbstractEditionCommand
    {
        public float frictionCoeff { get; set; }

        public float reboundCoeff { get; set; }

        public float accCoeff { get; set; }

        public CoefficientCommand(string objectUuid) : base(objectUuid)
        {
        }
    }
}
