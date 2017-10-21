using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InterfaceGraphique.Entities;
using Microsoft.AspNet.SignalR.Client;
using System.Threading;

namespace InterfaceGraphique.CommunicationInterface.WaitingRooms
{
    public class TournamentWaitingRoomHub : WaitingRoomHub
    {
        private bool test = false;

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
            if (!test)
            {
                InitializeEvents();
                test = true;
            }
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
            
            base.InitializeEvent();
        }

        private void InitializeConfigurationEvents()
        {
            WaitingRoomProxy.On<TournamentEntity>("TournamentStarting", officialGame =>
            {
                // CREATE NEW GAME
            });

            WaitingRoomProxy.On<MapEntity>("TournamentMapUpdatedEvent", map =>
            {
                CurrentTournament.SelectedMap = map;
                InvokeMapUpdated(map);

            });
            
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
