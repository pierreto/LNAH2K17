using AirHockeyServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirHockeyServer.Events
{
    ///////////////////////////////////////////////////////////////////////////////
    /// @file MatchFoundArgs.cs
    /// @author Ariane Tourangeau
    /// @date 2017-10-02
    /// @version 0.1
    ///
    /// Cette classe représente les arguments utilisés lorsqu'un event est lancé
    ///////////////////////////////////////////////////////////////////////////////
    public class MatchFoundArgs
    {
        public GameEntity GameEntity { get; set; }
    }
}