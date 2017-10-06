using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using InterfaceGraphique.Entities;
using InterfaceGraphique.Game.GameState;
using Microsoft.AspNet.SignalR.Client;

namespace InterfaceGraphique.CommunicationInterface
{
    public class WaitingRoomHub : IBaseHub
    {
        private IHubProxy GameWaitingRoomProxy { get; set; }

        private HubConnection connection;
        private SlaveGameState slaveGameState;
        private string username;

        public event EventHandler<int> RemainingTimeEvent;

        public event EventHandler<UserEntity> OpponentFoundEvent;

        public event EventHandler<GameEntity> MapUpdatedEvent;

        public event EventHandler<GameEntity> ConfigurationUpdatedEvent;

        public event EventHandler<GameEntity> GameStartingEvent;

        public WaitingRoomHub(SlaveGameState slaveGameState)
        {
            this.slaveGameState = slaveGameState;
        }

        public void InitializeHub(HubConnection connection, string username)
        {
            this.connection = connection;
            this.username = username;
            GameWaitingRoomProxy = this.connection.CreateHubProxy("GameWaitingRoomHub");
        }

        public void Logout()
        {
            //TODO: IMPLEMENT THE LOGOUT MECANISM
        }

        public void Cancel()
        {
            //TODO: IMPLEMENT THIS
        }

        public async void JoinGame()
        {
            InitializeEvents();

            UserEntity user = new UserEntity
            {
                Id = Guid.NewGuid(),
                Name = "pepe"
            };
            await GameWaitingRoomProxy.Invoke("JoinGame", user);
        }

        private void InitializeEvents()
        {
            GameWaitingRoomProxy.On<GameEntity>("OpponentFoundEvent", newgame =>
            {
                this.OpponentFoundEvent.Invoke(this, newgame.Players[0]);
                GameWaitingRoomProxy.On<GameEntity>("GameStartingEvent", officialGame =>
                {
                    Console.WriteLine("Game is starting!");
                });

                GameWaitingRoomProxy.On<int>("WaitingRoomRemainingTime", remainingTime =>
                {
                    this.RemainingTimeEvent.Invoke(this, remainingTime);
                });

                GameWaitingRoomProxy.On<GameEntity>("GameConfigurationUpdatedEvent", gameUpdated2 =>
                {
                    Console.WriteLine("configuration updated");
                });

                GameWaitingRoomProxy.On<GameEntity>("GameMapUpdatedEvent", mapUpdated =>
                {
                    Console.WriteLine("map updated");
                });
                /*
                if (this.username.Equals(newgame.Master.Name))
                {
                    Program.QuickPlay.CurrentGameState = this.slaveGameState;
                }
                else
                {
                    Program.QuickPlay.CurrentGameState = this.slaveGameState;
        
                }
                Program.FormManager.CurrentForm = Program.QuickPlay;*/

            });
        }

        public async Task<GameEntity> UpdateSelectedMap(GameEntity game)
        {
            GameEntity gameUpdated = await GameWaitingRoomProxy.Invoke<GameEntity>("UpdateMap", game);
            return gameUpdated;
        }

        public async Task<GameEntity> UpdateSelectedConfiguration(GameEntity game)
        {
            GameEntity gameUpdated = await GameWaitingRoomProxy.Invoke<GameEntity>("UpdateConfiguration", game);
            return gameUpdated;
        }
    }
}
