using System.Collections.Generic;
using System.Threading.Tasks;
using AirHockeyServer.Entities;
using Microsoft.AspNet.SignalR;
using AirHockeyServer.Hubs;

namespace AirHockeyServer.Services.ChatServiceServer
{
    public class EditionBrowserHub : BaseHub
    {
        private EditionService editionService;
        // Should be thread-safe?
        public EditionBrowserHub(EditionService editionService, ConnectionMapper connectionMapper)
            : base(connectionMapper)
        {
            this.editionService = editionService;


            this.editionService.AvailableMapInfos.Add(new OnlineEditedMapInfo()
            {
                IsPublic = true,
                MapName = "Map à supprimer",
                MaxNumberOfPlayer = 4,
                NumberOfPlayer = 0,
                OnlineGroupId = "1231231asdfasdf23312"
            });
        }
        public void NewMapEdited(OnlineEditedMapInfo mapInfo)
        {
            //Save it on the database?

            this.editionService.AvailableMapInfos.Add(mapInfo);
            //Inform all user that a new map has been created and can be joined
            Clients.All.NewMap(mapInfo);
        }

        public void MapIsFull(OnlineEditedMapInfo mapInfo)
        {
            this.editionService.AvailableMapInfos.Remove(mapInfo);
            //Inform all user that a the map is full and can't be joined
            Clients.All.MapIsFull(mapInfo);
        }

        public List<OnlineEditedMapInfo> GetAvailableMapList()
        {
            return this.editionService.AvailableMapInfos;
        }

        public void Disconnect()
        {
            base.Disconnect();
            // TODO
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            // TODO ANY SPECIAL ACTION?

            return base.OnDisconnected(stopCalled);
        }
    }
}