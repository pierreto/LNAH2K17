using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using InterfaceGraphique.Entities;
using Microsoft.AspNet.SignalR.Client;

namespace InterfaceGraphique.CommunicationInterface
{
    class GameLobbyHub : IBaseHub
    {
        private IHubProxy GameWaitingRoomProxy { get; set; }

        private HubConnection connection;
        private string username;

        public void InitializeHub(HubConnection connection, string username)
        {
            this.connection = connection;
            this.username = username;
            GameWaitingRoomProxy = this.connection.CreateHubProxy("GameWaitingRoomHub");
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }

        public async void test()
        {
            GameEntity game = new GameEntity
            {
                Creator = new UserEntity
                {
                    Id = Guid.NewGuid()
                }
            };

            Console.WriteLine("Game created");
            await GameWaitingRoomProxy.Invoke("CreateGame", game);

            GameWaitingRoomProxy.On<GameEntity>("OpponentFoundEvent", newgame =>
            {
                Console.WriteLine("Opponent found");
                GameWaitingRoomProxy.On<GameEntity>("GameStartingEvent", officialGame =>
                {
                    Console.WriteLine("Game is starting!");
                });

                GameWaitingRoomProxy.On<int>("WaitingRoomRemainingTime", remainingTime =>
                {
                    Console.WriteLine(remainingTime);
                });

                GameWaitingRoomProxy.On<GameEntity>("GameConfigurationUpdatedEvent", gameUpdated2 =>
                {
                    Console.WriteLine("configuration updated");
                });

                GameWaitingRoomProxy.On<GameEntity>("GameMapUpdatedEvent", mapUpdated =>
                {
                    Console.WriteLine("map updated");
                });
            });

            Thread.Sleep(5000);

            UserEntity user = new UserEntity
            {
                Id = Guid.NewGuid()
            };

            await GameWaitingRoomProxy.Invoke("JoinGame", user);

            Thread.Sleep(5000);

            var gameUpdated = await GameWaitingRoomProxy.Invoke<GameEntity>("UpdateConfiguration", game);

            Thread.Sleep(2000);

            gameUpdated = await GameWaitingRoomProxy.Invoke<GameEntity>("UpdateMap", game);
        }

  
    }
}
