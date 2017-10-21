using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InterfaceGraphique.Entities;
using Microsoft.AspNet.SignalR.Client;

namespace InterfaceGraphique.CommunicationInterface.WaitingRooms
{
    public class TournamentWaitingRoomHub : WaitingRoomHub, IBaseHub
    {
        protected TournamentEntity CurrentTournament { get; set; }

        public event EventHandler<UserEntity> OpponentFoundEvent;

        public event EventHandler<TournamentEntity> TournamentAllOpponentsFound;

        public override void InitializeHub(HubConnection connection, string username)
        {
            base.InitializeHub(connection, username);
            WaitingRoomProxy = connection.CreateHubProxy("TournamentWaitingRoomHub");
        }

        public override void Join()
        {
            InitializeEvents();
            base.Join();
        }
        private void InitializeEvents()
        {
            WaitingRoomProxy.On<UserEntity>("OpponentFoundEvent", (opponent) =>
            {
                this.OpponentFoundEvent.Invoke(this, opponent);
                
            });

            WaitingRoomProxy.On<TournamentEntity>("TournamentAllOpponentsFound", (tournament) =>
            {
                this.TournamentAllOpponentsFound.Invoke(this, tournament);
                CurrentTournament = tournament;
                InitializeConfigurationEvents();
            });
        }

        private void InitializeConfigurationEvents()
        {
            WaitingRoomProxy.On<TournamentEntity>("TournamentStartingEvent", officialGame =>
            {

            });

            WaitingRoomProxy.On<MapEntity>("TournamentMapUpdatedEvent", map =>
            {
                CurrentTournament.SelectedMap = map;
                InvokeMapUpdated(map);

            });

            base.InitializeEvent();
        }

        public override void Logout()
        {
            WaitingRoomProxy?.Invoke("Disconnect", this.Username).Wait();
        }

        public async void UpdateSelectedMap(MapEntity map)
        {
            if (CurrentTournament != null)
            {
                CurrentTournament.SelectedMap = map;
                CurrentTournament = await WaitingRoomProxy.Invoke<TournamentEntity>("UpdateMap", CurrentTournament);
            }
        }

        public void LeaveTournament()
        {
            WaitingRoomProxy.Invoke("LeaveTournament", user);
        }

    }
}
