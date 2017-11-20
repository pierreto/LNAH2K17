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
using InterfaceGraphique.Services;

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

        public GameManager GameManager { get; }

        public GameWaitingRoomHub(SlaveGameState slaveGameState, MasterGameState masterGameState,
            GameManager gameManager)
        {
            this.slaveGameState = slaveGameState;
            this.masterGameState = masterGameState;
            GameManager = gameManager;
        }

        public void InitializeHub(HubConnection connection)
        {
            this.HubConnection = connection;
            WaitingRoomProxy = this.HubConnection.CreateHubProxy("GameWaitingRoomHub");
            InitializeEvents();
        }

        public async void Join()
        {
            await WaitingRoomProxy.Invoke("Join", User.Instance.UserEntity);
        }

        public async Task LeaveGame()
        {
            await WaitingRoomProxy.Invoke("LeaveGame", User.Instance.UserEntity, CurrentGameId);
        }

        public async void UpdateSelectedMap(MapEntity map)
        {
            try
            {
                await WaitingRoomProxy.Invoke("UpdateMap", CurrentGameId, map);
            }
            catch (Exception e)
            { }
        }

        private void InitializeEvents()
        {
            WaitingRoomProxy.On<GameEntity>("OpponentFoundEvent", newgame => OnOpponentFound(newgame));
            { };

            WaitingRoomProxy.On<GameEntity>("GameStartingEvent", officialGame => OnGameStarting(officialGame));
            { };

            WaitingRoomProxy.On<MapEntity>("GameMapUpdatedEvent", mapUpdated => OnMapUpdated(mapUpdated));
            { };

            WaitingRoomProxy.On<int>("WaitingRoomRemainingTime", remainingTime => OnRemainingTime(remainingTime));
            { };
        }

        public void OnOpponentFound(GameEntity game)
        {
            this.CurrentGameId = game.GameId;
            this.OpponentFoundEvent.Invoke(this, game);
        }

        public void OnGameStarting(GameEntity game)
        {
            CurrentGameId = new Guid();
            Program.LobbyHost.Invoke(new MethodInvoker(async () =>
            {
                GameManager.CurrentOnlineGame = game;
                await GameManager.SetTextures();

                if (User.Instance.UserEntity.Id == game.Master.Id)
                {
                    this.masterGameState.InitializeGameState(game);

                    Program.QuickPlay.CurrentGameState = this.masterGameState;
                    Program.QuickPlay.CurrentGameState.IsOnline = true;
                    Program.FormManager.CurrentForm = Program.QuickPlay;
                }
                else
                {
                    this.slaveGameState.InitializeGameState(game);

                    Program.QuickPlay.CurrentGameState = this.slaveGameState;
                    Program.QuickPlay.CurrentGameState.IsOnline = true;
                    Program.FormManager.CurrentForm = Program.QuickPlay;

                    FonctionsNatives.rotateCamera(180);
                }

                Program.QuickPlay.CurrentGameState.ApplyTextures();
            }));
        }

        public void OnMapUpdated(MapEntity map)
        {
            this.MapUpdatedEvent.Invoke(this, map);
        }

        public void OnRemainingTime(int remainingTime)
        {
            this.RemainingTimeEvent.Invoke(this, remainingTime);
        }

        public async Task Logout()
        {
            //TODO: IMPLEMENT THE LOGOUT MECANISM
        }

    }
}
