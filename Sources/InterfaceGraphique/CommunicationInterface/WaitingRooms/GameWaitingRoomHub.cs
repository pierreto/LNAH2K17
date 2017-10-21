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
    public class GameWaitingRoomHub : WaitingRoomHub
    {
        private SlaveGameState slaveGameState;

        private MasterGameState masterGameState;

        protected GameEntity CurrentGame { get; set; }

        public event EventHandler<GameEntity> OpponentFoundEvent;

        public GameWaitingRoomHub(SlaveGameState slaveGameState,MasterGameState masterGameState) : base()
        {
            this.slaveGameState = slaveGameState;
            this.masterGameState = masterGameState;
        }

        public override void InitializeHub(HubConnection connection, string username)
        {
            base.InitializeHub(connection, username);
            WaitingRoomProxy = this.HubConnection.CreateHubProxy("GameWaitingRoomHub");
        }
        
        public override void Join()
        {
            InitializeEvents();
            base.Join();
        }
        
        public async Task LeaveGame()
        {
            await WaitingRoomProxy.Invoke("LeaveGame", user);
        }

        private void InitializeEvents()
        {
            WaitingRoomProxy.On<GameEntity>("OpponentFoundEvent", newgame =>
            {
                this.CurrentGame = newgame;
                this.OpponentFoundEvent.Invoke(this, newgame);

                WaitingRoomProxy.On<GameEntity>("GameStartingEvent", officialGame =>
                {
                    Console.WriteLine("Game is starting!");

                    Program.LobbyHost.Invoke(new MethodInvoker(() =>
                    {

                        if (this.Username.Equals(officialGame.Master.Username))
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
                WaitingRoomProxy.On<GameEntity>("GameMapUpdatedEvent", mapUpdated =>
                {
                    this.CurrentGame = mapUpdated;
                    InvokeMapUpdated(mapUpdated.SelectedMap);
                    
                });

                base.InitializeEvent();
            });
        }

        public async void UpdateSelectedMap(MapEntity map)
        {
            if (CurrentGame != null)
            {
                CurrentGame.SelectedMap = map;
                CurrentGame = await WaitingRoomProxy.Invoke<GameEntity>("UpdateMap", CurrentGame);
            }
        }

    }
}
