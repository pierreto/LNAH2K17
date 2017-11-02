﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceGraphique.Entities.EditorCommand;

namespace InterfaceGraphique.Entities.EditonCommand
{
    class MoveCommand : AbstractEditionCommand
    {
        public float[] Position { get; set; }

        public MoveCommand(string uuid ) : base(uuid)
        {
        }

        public override void ExecuteCommand()
        {
            FonctionsNatives.moveByUUID(objectUuid, Position);
        }
    }
}