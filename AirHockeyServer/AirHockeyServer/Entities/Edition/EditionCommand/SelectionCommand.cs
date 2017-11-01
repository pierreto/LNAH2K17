using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirHockeyServer.Entities.Messages.Edition;

namespace AirHockeyServer.Entities.Edition.EditionCommand
{
    class SelectionCommand : AbstractEditionCommand
    {
        public string Username { get; set; }
        public SelectionCommand(string objectUuid) : base(objectUuid)
        {
        }
    }
}
