using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirHockeyServer.Hubs
{
    public class ConnectionMapper
    {
        private static Dictionary<Guid, string> ConnectionsMapping = new Dictionary<Guid, string>();
        
        public bool AddConnection(Guid userId, string connection)
        {
            if ( !ConnectionsMapping.ContainsKey(userId))
            {
                ConnectionsMapping.Add(userId, connection);
                return true;
            }

            return false;
        }
    }
}