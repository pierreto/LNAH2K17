using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirHockeyServer.Pocos
{
    public abstract class Poco
    {
        abstract public int? Id { get; set; }
    }
}