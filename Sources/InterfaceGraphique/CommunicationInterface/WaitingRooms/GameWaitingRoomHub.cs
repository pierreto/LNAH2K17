using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using InterfaceGraphique.Entities;
using InterfaceGraphique.Game.GameState;
using Microsoft.AspNet.SignalR.Client;

namespace InterfaceGraphique.CommunicationInterface.WaitingRooms
{
    public class GameWaitingRoomHub : IBaseHub
    {
        private SlaveGameState slaveGameState;

        private MasterGameState masterGameState;

        protected Guid CurrentGameId { get; set; }

        public event EventHandler<GameEntity> OpponentFoundEvent;

        public event EventHandler<int> RemainingTimeEvent;

        public event EventHandler<MapEntity> MapUpdatedEvent;

        public static IHubProxy WaitingRoomProxy { get; set; }

        protected HubConnection HubConnection { get; set; }

        public GameWaitingRoomHub(SlaveGameState slaveGameState,MasterGameState masterGameState)
        {
            this.slaveGameState = slaveGameState;
            this.masterGameState = masterGameState;
        }

        public void InitializeHub(HubConnection connection)
        {
            this.HubConnection = connection;
            WaitingRoomProxy = this.HubConnection.CreateHubProxy("GameWaitingRoomHub");
        }
        
        public async void Join()
        {
            InitializeEvents();
            
            await WaitingRoomProxy.Invoke("Join", User.Instance.UserEntity);
        }
        
        public async Task LeaveGame()
        {
            await WaitingRoomProxy.Invoke("LeaveGame", User.Instance.UserEntity, CurrentGameId);
        }

        private void InitializeEvents()
        {
            WaitingRoomProxy.On<GameEntity>("OpponentFoundEvent", newgame =>
            {
                this.CurrentGameId = newgame.GameId;
                this.OpponentFoundEvent.Invoke(this, newgame);

                WaitingRoomProxy.On<GameEntity>("GameStartingEvent", officialGame =>
                {
                    Console.WriteLine("Game is starting!");

                    Program.LobbyHost.Invoke(new MethodInvoker(() =>
                    {
                        if (User.Instance.UserEntity.Id == officialGame.Master.Id)
                        {
                            this.masterGameState.InitializeGameState(officialGame);

                            Program.QuickPlay.CurrentGameState = this.masterGameState;
                            Program.FormManager.CurrentForm = Program.QuickPlay;
                        }
                        else
                        {
                            this.slaveGameState.InitializeGameState(officialGame);

                            Program.QuickPlay.CurrentGameState = this.slaveGameState;
                            Program.FormManager.CurrentForm = Program.QuickPlay;

                            FonctionsNatives.rotateCamera(180);
                        }
                    }));
                });
                WaitingRoomProxy.On<MapEntity>("GameMapUpdatedEvent", mapUpdated =>
                {
                    this.MapUpdatedEvent.Invoke(this, mapUpdated);                    
                });

                WaitingRoomProxy.On<int>("WaitingRoomRemainingTime", remainingTime =>
                {
                    this.RemainingTimeEvent.Invoke(this, remainingTime);
                });
            });
        }

        public async void UpdateSelectedMap(MapEntity map)
        {
            await WaitingRoomProxy.Invoke("UpdateMap", CurrentGameId, map);
        }
        public async Task Logout()
        {
            //TODO: IMPLEMENT THE LOGOUT MECANISM
        }

    }
}
