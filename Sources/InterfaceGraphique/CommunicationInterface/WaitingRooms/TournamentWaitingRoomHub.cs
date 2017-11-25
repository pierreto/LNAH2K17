using System;
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
    public class TournamentWaitingRoomHub : BaseHub, IBaseHub
    {
        protected int CurrentTournamentId { get; set; }

        public event EventHandler<List<GamePlayerEntity>> OpponentFoundEvent;

        public event EventHandler<TournamentEntity> TournamentAllOpponentsFound;

        public event EventHandler<int> RemainingTimeEvent;

        public event EventHandler<MapEntity> MapUpdatedEvent;

        public event EventHandler<List<GamePlayerEntity>> SemiFinalResultEvent;

        public event EventHandler<GamePlayerEntity> WinnerResultEvent;

        public event EventHandler PlayerLeftEvent;

        public static IHubProxy WaitingRoomProxy { get; set; }

        protected HubConnection HubConnection { get; set; }

        protected SlaveGameState SlaveGameState { get; set; }

        protected MasterGameState MasterGameState { get; set; }

        public GameManager GameManager { get; set; }

        public TournamentWaitingRoomHub(SlaveGameState slaveGameState, MasterGameState masterGameState, GameManager gameManager)
        {
            SlaveGameState = slaveGameState;
            MasterGameState = masterGameState;
            GameManager = gameManager;
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
            List<GamePlayerEntity> players = new List<GamePlayerEntity>() { new GamePlayerEntity(User.Instance.UserEntity) };
            try
            {
                await WaitingRoomProxy.Invoke("Join", players);
            }
            catch (Exception e)
            {
                HandleError();
            }
        }

        public async void CreateTournament(List<GamePlayerEntity> players)
        {
            try
            {
                await WaitingRoomProxy.Invoke("Join", players);
            }
            catch (Exception e)
            {
                HandleError();
            }
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

        public async void UpdateSelectedMap(MapEntity map)
        {
            if (CurrentTournamentId > 0)
            {
                try
                {
                    await WaitingRoomProxy.Invoke<TournamentEntity>("UpdateMap", CurrentTournamentId, map);
                }
                catch (Exception e)
                {
                    HandleError();
                }
            }
        }

        public async Task LeaveTournament()
        {
            try
            {
                await WaitingRoomProxy.Invoke("LeaveTournament", User.Instance.UserEntity, CurrentTournamentId);
            }
            catch (Exception e)
            {
                HandleError();
            }

            CurrentTournamentId = 0;
        }

        private void InitializeTournamentsEvents()
        {
            WaitingRoomProxy.On<TournamentEntity>("TournamentStarting", tournament => OnTournamentStarting(tournament));

            WaitingRoomProxy.On<TournamentEntity>("StartFinal", tournament => OnFinalStarting(tournament));

            WaitingRoomProxy.On<TournamentEntity>("TournamentSemiFinalResults", tournament => OnSemiFinalResults(tournament));

            WaitingRoomProxy.On<TournamentEntity>("TournamentFinalResult", tournament => OnFinalResults(tournament));

            WaitingRoomProxy.On("PlayerLeft", () => OnPlayerLeft());
        }

        private void InitializeWaitingRoomEvents()
        {
            WaitingRoomProxy.On<List<GamePlayerEntity>>("OpponentFoundEvent", (opponents) => OnOpponentFoundEvent(opponents));

            WaitingRoomProxy.On<TournamentEntity>("TournamentAllOpponentsFound", (tournament) => OnAllOpponentsFound(tournament));

            WaitingRoomProxy.On<int>("WaitingRoomRemainingTime", remainingTime => OnRemainingTime(remainingTime));

            WaitingRoomProxy.On<MapEntity>("TournamentMapUpdatedEvent", map => OnMapUpdated(map));
        }

        public void OnOpponentFoundEvent(List<GamePlayerEntity> opponents)
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
            this.RemainingTimeEvent?.Invoke(this, remainingTime);
        }

        public void OnMapUpdated(MapEntity map)
        {
            this.MapUpdatedEvent?.Invoke(this, map);
        }

        public void OnSemiFinalResults(TournamentEntity tournament)
        {
            SemiFinalResultEvent?.Invoke(this, new List<GamePlayerEntity>() { tournament.SemiFinals[0].Winner, tournament.SemiFinals[1].Winner });
        }

        public void OnFinalResults(TournamentEntity tournament)
        {
            WinnerResultEvent?.Invoke(this, tournament.Final.Winner);
        }

        public void OnPlayerLeft()
        {
            PlayerLeftEvent?.Invoke(this, null);
        }

        public void OnFinalStarting(TournamentEntity tournament)
        {
            Program.OnlineTournament.Invoke(new MethodInvoker(() =>
            {
                if (tournament.Final.Players[0].Id == User.Instance.UserEntity.Id || tournament.Final.Players[1].Id == User.Instance.UserEntity.Id)
                {
                    SetGame(tournament.Final, tournament.Final.Master.Id == User.Instance.UserEntity.Id);
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

            }));
        }

        private void SetGame(GameEntity game, bool isMaster)
        {
            Program.OnlineTournament.Invoke(new MethodInvoker(async () =>
            {
                GameManager.CurrentOnlineGame = game;
                await GameManager.SetTextures();

                if (isMaster)
                {
                    this.MasterGameState.InitializeGameState(game);
                    Program.QuickPlay.CurrentGameState = this.MasterGameState;

                    if (game.Slave.IsAi)
                    {
                        FonctionsNatives.setCurrentOpponentType((int)OpponentType.VIRTUAL_PLAYER);
                    }

                    Program.FormManager.CurrentForm = Program.QuickPlay;
                    Program.QuickPlay.CurrentGameState.IsOnlineTournementMode = true;

                }
                else
                {
                    this.SlaveGameState.InitializeGameState(game);
                    Program.QuickPlay.CurrentGameState = this.SlaveGameState;

                    Program.FormManager.CurrentForm = Program.QuickPlay;
                    Program.QuickPlay.CurrentGameState.IsOnlineTournementMode = true;
                    FonctionsNatives.rotateCamera(180);

                }

                Program.QuickPlay.CurrentGameState.ApplyTextures();
            }));
        }

        public async Task LeaveRoom()
        {
            await this.LeaveTournament();
        }
    }
}
