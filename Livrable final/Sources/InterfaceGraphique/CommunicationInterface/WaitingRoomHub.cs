﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using InterfaceGraphique.Entities;
using InterfaceGraphique.Game.GameState;
using Microsoft.AspNet.SignalR.Client;

namespace InterfaceGraphique.CommunicationInterface
{
    public class WaitingRoomHub : IBaseHub
    {
        public static IHubProxy GameWaitingRoomProxy { get; set; }

        private HubConnection connection;
        private SlaveGameState slaveGameState;
        private MasterGameState masterGameState;

        private string username;

        public event EventHandler<int> RemainingTimeEvent;

        public event EventHandler<UserEntity> OpponentFoundEvent;

        public event EventHandler<GameEntity> MapUpdatedEvent;

        public event EventHandler<GameEntity> ConfigurationUpdatedEvent;

        public event EventHandler<GameEntity> GameStartingEvent;

        public WaitingRoomHub(SlaveGameState slaveGameState,MasterGameState masterGameState)
        {
            this.slaveGameState = slaveGameState;
            this.masterGameState = masterGameState;
        }

        public void InitializeHub(HubConnection connection)
        {
            this.connection = connection;
            GameWaitingRoomProxy = this.connection.CreateHubProxy("GameWaitingRoomHub");
        }

        public async Task Logout()
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
            await GameWaitingRoomProxy.Invoke("JoinGame", User.Instance.UserEntity);
        }

        private void InitializeEvents()
        {
            GameWaitingRoomProxy.On<GameEntity>("OpponentFoundEvent", newgame =>
            {
                GameWaitingRoomProxy.On<GameEntity>("GameStartingEvent", officialGame =>
                {
                    Console.WriteLine("Game is starting!");
                    Program.LobbyHost.Invoke(new MethodInvoker(() =>
                    {

                        if (this.username.Equals(officialGame.Master.Username))
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
