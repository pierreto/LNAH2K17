using AirHockeyServer.Core;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AirHockeyServer.Hubs
{
    public class BaseHub : Hub
    {
        protected BaseHub(ConnectionMapper connectionMapper)
        {
            ConnectionMapper = connectionMapper;
        }

        public ConnectionMapper ConnectionMapper { get; }

        public override Task OnDisconnected(bool stopCalled)
        {
            Disconnect();

            return base.OnDisconnected(stopCalled);
        }

        public virtual void Disconnect()
        {
            var userId = ConnectionMapper.GetIdFromConnection(Context.ConnectionId);

            Cache.RemovePlayer(userId);
            ConnectionMapper.DeleteConnection(userId);
            ConnectionMapper.RemoveUserConnection(Context.ConnectionId);
        }
    }
}