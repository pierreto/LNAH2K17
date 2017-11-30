using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirHockeyServer.Entities.Messages.Edition;

namespace AirHockeyServer.Entities.Edition.EditionCommand
{
    public class DeleteCommand : AbstractEditionCommand
    {
        public string Username { get; set; }
        public DeleteCommand(string objectUuid) : base(objectUuid)
        {
        }
    }
}
