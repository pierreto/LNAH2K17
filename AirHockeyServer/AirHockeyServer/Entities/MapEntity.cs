using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirHockeyServer.Entities
{
    ///////////////////////////////////////////////////////////////////////////////
    /// @file MapEntity.cs
    /// @author Michael Sghaier
    /// @date 2017-10-02
    /// @version 0.1
    ///
    /// Cette classe représente une carte de jeu
    ///////////////////////////////////////////////////////////////////////////////
    public class MapEntity : Entity
    {
        public string Creator { get; set; }
        public string MapName { get; set; }
        public DateTime LastBackup { get; set; }
        public string Json { get; set; }
    }
}