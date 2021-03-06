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
using InterfaceGraphique.Services;
using InterfaceGraphique.Controls.WPF.Friends;
using Microsoft.Practices.Unity;

namespace InterfaceGraphique.CommunicationInterface.WaitingRooms
{
    public class GameWaitingRoomHub : BaseHub, IBaseHub
    {
        private SlaveGameState slaveGameState;

        private MasterGameState masterGameState;

        protected Guid CurrentGameId { get; set; }

        public event EventHandler<GameEntity> OpponentFoundEvent;

        public event EventHandler<int> RemainingTimeEvent;

        public event EventHandler<MapEntity> MapUpdatedEvent;

        public event EventHandler<int> OpponentLeftEvent;

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
            GamePlayerEntity player = new GamePlayerEntity(User.Instance.UserEntity);
            try
            {
                await WaitingRoomProxy.Invoke("Join", player);
            }
            catch (Exception e)
            {
                HandleError();
            }
        }

        public async Task LeaveGame()
        {
            try
            {
                User.Instance.UserEntity.IsPlaying = false;
                Program.unityContainer.Resolve<FriendListViewModel>().OnPropertyChanged("CanShowPlay");
                if (Program.unityContainer.Resolve<FriendListViewModel>().FriendList != null)
                {
                    foreach (FriendListItemViewModel flivm in Program.unityContainer.Resolve<FriendListViewModel>().FriendList)
                    {
                        flivm.OnPropertyChanged("CanSendPlay");
                    }
                }
                await WaitingRoomProxy.Invoke("LeaveGame", User.Instance.UserEntity.Id, CurrentGameId);
            }
            catch (Exception e)
            {
                HandleError();
            }
        }

        public async Task UpdateSelectedMap(MapEntity map)
        {
            try
            {
                await WaitingRoomProxy.Invoke("UpdateMap", CurrentGameId, map);
            }
            catch (Exception e)
            {
                HandleError();
            }
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

            WaitingRoomProxy.On<int>("PlayerLeft", playerId => OnOpponentLeft(playerId));
        }

        private void OnOpponentLeft(int playerId)
        {
            this.OpponentLeftEvent?.Invoke(this, playerId);
        }

        public void OnOpponentFound(GameEntity game)
        {
            this.CurrentGameId = game.GameId;
            this.OpponentFoundEvent?.Invoke(this, game);
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
            try
            {
                await WaitingRoomProxy.Invoke("Disconnect");
            }
            catch (Exception e)
            {
                HandleError();
            }
        }

        public async Task LeaveRoom()
        {
            await this.LeaveGame();
        }

        public void OnDisconnect()
        {
            Program.FormManager.Invoke(new MethodInvoker(async () =>
            {
                Program.QuickPlay.Restart();
            }));

        }
    }
}
