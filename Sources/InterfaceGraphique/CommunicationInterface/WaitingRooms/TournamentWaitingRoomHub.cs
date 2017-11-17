﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InterfaceGraphique.Entities;
using Microsoft.AspNet.SignalR.Client;
using System.Threading;
using InterfaceGraphique.Game.GameState;
using System.Drawing;

namespace InterfaceGraphique.CommunicationInterface.WaitingRooms
{
    public class TournamentWaitingRoomHub : IBaseHub
    {
        protected int CurrentTournamentId { get; set; }

        public event EventHandler<List<UserEntity>> OpponentFoundEvent;

        public event EventHandler<TournamentEntity> TournamentAllOpponentsFound;

        public event EventHandler<int> RemainingTimeEvent;

        public event EventHandler<MapEntity> MapUpdatedEvent;

        public event EventHandler<List<UserEntity>> SemiFinalResultEvent;

        public event EventHandler<UserEntity> WinnerResultEvent;

        public static IHubProxy WaitingRoomProxy { get; set; }

        protected HubConnection HubConnection { get; set; }

        protected SlaveGameState SlaveGameState { get; set; }

        protected MasterGameState MasterGameState { get; set; }

        public TournamentWaitingRoomHub(SlaveGameState slaveGameState, MasterGameState masterGameState)
        {
            SlaveGameState = slaveGameState;
            MasterGameState = masterGameState;
        }

        public void InitializeHub(HubConnection connection)
        {
            this.HubConnection = connection;
            WaitingRoomProxy = connection.CreateHubProxy("TournamentWaitingRoomHub");
            InitializeWaitingRoomEvents();
            InitializeTournamentsEvents();
        }

        public async void Join()
        {
            await WaitingRoomProxy.Invoke("Join", User.Instance.UserEntity);
        }

        public async Task Logout()
        {
            await WaitingRoomProxy.Invoke("Disconnect", User.Instance.UserEntity.Username);
        }

        public async void UpdateSelectedMap(MapEntity map)
        {
            if (CurrentTournamentId > 0)
            {
                await WaitingRoomProxy.Invoke<TournamentEntity>("UpdateMap", CurrentTournamentId, map);
            }
        }

        public async Task LeaveTournament()
        {
            await WaitingRoomProxy.Invoke("LeaveTournament", User.Instance.UserEntity, CurrentTournamentId);
        }

        private void InitializeTournamentsEvents()
        {
            WaitingRoomProxy.On<TournamentEntity>("TournamentStarting", tournament => OnTournamentStarting(tournament));

            WaitingRoomProxy.On<TournamentEntity>("StartFinal", tournament => OnFinalStarting(tournament));

            WaitingRoomProxy.On<TournamentEntity>("TournamentSemiFinalResults", tournament => OnSemiFinalResults(tournament));

            WaitingRoomProxy.On<TournamentEntity>("TournamentFinalResult", tournament => OnFinalResults(tournament));
        }
        
        private void InitializeWaitingRoomEvents()
        {
            WaitingRoomProxy.On<List<UserEntity>>("OpponentFoundEvent", (opponents) => OnOpponentFoundEvent(opponents));

            WaitingRoomProxy.On<TournamentEntity>("TournamentAllOpponentsFound", (tournament) => OnAllOpponentsFound(tournament));

            WaitingRoomProxy.On<int>("WaitingRoomRemainingTime", remainingTime => OnRemainingTime(remainingTime));

            WaitingRoomProxy.On<MapEntity>("TournamentMapUpdatedEvent", map => OnMapUpdated(map));
        }

        public void OnOpponentFoundEvent(List<UserEntity> opponents)
        {
            this.OpponentFoundEvent.Invoke(this, opponents);
        }

        public void OnAllOpponentsFound(TournamentEntity tournament)
        {
            this.TournamentAllOpponentsFound.Invoke(this, tournament);
            CurrentTournamentId = tournament.Id;
        }

        public void OnRemainingTime(int remainingTime)
        {
            this.RemainingTimeEvent.Invoke(this, remainingTime);
        }

        public void OnMapUpdated(MapEntity map)
        {
            this.MapUpdatedEvent.Invoke(this, map);
        }

        public void OnSemiFinalResults(TournamentEntity tournament)
        {
            SemiFinalResultEvent.Invoke(this, new List<UserEntity>() { tournament.SemiFinals[0].Winner, tournament.SemiFinals[1].Winner });
        }

        public void OnFinalResults(TournamentEntity tournament)
        {
            WinnerResultEvent.Invoke(this, tournament.Final.Winner);
        }

        public void OnFinalStarting(TournamentEntity tournament)
        {
            CurrentTournamentId = 0;
            Program.OnlineTournament.Invoke(new MethodInvoker(() =>
            {
                if (tournament.Final.Players[0].Id == User.Instance.UserEntity.Id || tournament.Final.Players[1].Id == User.Instance.UserEntity.Id)
                {
                    SetGame(tournament.Final, tournament.Final.Master.Id == User.Instance.UserEntity.Id);
                }
                else
                {
                    Program.FormManager.CurrentForm = Program.MainMenu;
                }

            }));
        }

        public void OnTournamentStarting(TournamentEntity tournament)
        {
            Program.OnlineTournament.Invoke(new MethodInvoker(() =>
            {
                var userGame = tournament.SemiFinals.Find(game => game.Players.Any(player => player.Id == User.Instance.UserEntity.Id));
                if (userGame != null)
                {
                    SetGame(userGame, userGame.Master.Id == User.Instance.UserEntity.Id);
                }
                else
                {
                    Program.FormManager.CurrentForm = Program.MainMenu;
                }

            }));
        }

        private void SetGame(GameEntity game,  bool isMaster)
        {
            Program.OnlineTournament.Invoke(new MethodInvoker(() =>
            {
                if (isMaster)
                {
                    this.MasterGameState.InitializeGameState(game);
                    this.MasterGameState.IsOnlineTournementMode = true;
                    Program.QuickPlay.CurrentGameState = this.MasterGameState;

                    Program.FormManager.CurrentForm = Program.QuickPlay;
                }
                else
                {
                    this.SlaveGameState.InitializeGameState(game);
                    this.MasterGameState.IsOnlineTournementMode = true;
                    Program.QuickPlay.CurrentGameState = this.SlaveGameState;

                    Program.FormManager.CurrentForm = Program.QuickPlay;

                    FonctionsNatives.rotateCamera(180);

                }

                Program.QuickPlay.CurrentGameState.IsTournementMode = false;
            }));
        }
    }
}
