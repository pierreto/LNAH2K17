using InterfaceGraphique.Entities;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceGraphique.CommunicationInterface.WaitingRooms
{
    public abstract class WaitingRoomHub : IBaseHub
    {
        
        public event EventHandler<int> RemainingTimeEvent;

        public event EventHandler<MapEntity> MapUpdatedEvent;

        public static IHubProxy WaitingRoomProxy { get; set; }

        public string Username { get; protected set; }

        protected UserEntity user { get; set; }

        protected HubConnection HubConnection { get; set; }

        public virtual void InitializeHub(HubConnection connection, string username)
        {
            this.HubConnection = connection;
            this.Username = username;

        }

        public virtual void Join()
        {
            Random random = new Random();
            user = new UserEntity
            {
                UserId = random.Next(),
                Username = Username
            };

            WaitingRoomProxy.Invoke("Join", user);
        }

        public virtual void Logout()
        {
            //TODO: IMPLEMENT THE LOGOUT MECANISM
        }

        public virtual void Cancel()
        {
            //TODO: IMPLEMENT THIS
        }

        protected void InitializeEvent()
        {
            WaitingRoomProxy.On<int>("WaitingRoomRemainingTime", remainingTime =>
            {
                this.RemainingTimeEvent.Invoke(this, remainingTime);
            });
        }

        protected void InvokeMapUpdated(MapEntity map)
        {
            this.MapUpdatedEvent.Invoke(this, map);
        }
    }
}
