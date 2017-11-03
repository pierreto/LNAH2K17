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
    public class TournamentWaitingRoomHub : IBaseHub
    {
        private bool test = false;

        protected TournamentEntity CurrentTournament { get; set; }

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

        }

        public async void Join()
        {
            InitializeWaitingRoomEvents();
            await WaitingRoomProxy.Invoke("Join", User.Instance.UserEntity);
        }

        public async Task Logout()
        {
            await WaitingRoomProxy.Invoke("Disconnect", User.Instance.UserEntity.Username);
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
            WaitingRoomProxy.Invoke("LeaveTournament", User.Instance.UserEntity);
        }

        private void InitializeWaitingRoomEvents()
        {
            InitializeTournamentsEvents();
            WaitingRoomProxy.On<List<UserEntity>>("OpponentFoundEvent", (opponents) =>
            {
                this.OpponentFoundEvent.Invoke(this, opponents);
            });

            WaitingRoomProxy.On<TournamentEntity>("TournamentAllOpponentsFound", (tournament) =>
            {
                this.TournamentAllOpponentsFound.Invoke(this, tournament);
                CurrentTournament = tournament;
            });

            WaitingRoomProxy.On<int>("WaitingRoomRemainingTime", remainingTime =>
            {
                this.RemainingTimeEvent.Invoke(this, remainingTime);
            });

            WaitingRoomProxy.On<MapEntity>("TournamentMapUpdatedEvent", map =>
            {
                //this.MapUpdatedEvent.Invoke(this, map);
            });
        }

        private void InitializeTournamentsEvents()
        {
            WaitingRoomProxy.On<TournamentEntity>("TournamentStarting", tournament =>
            {
                Program.OnlineTournament.Invoke(new MethodInvoker(() =>
                    {
                        var userGame = tournament.SemiFinals.Find(game => game.Players.Any(player => player.Id == User.Instance.UserEntity.Id));
                        if (userGame != null)
                        {
                            if (userGame.Master.Id == User.Instance.UserEntity.Id)
                            {
                                this.MasterGameState.InitializeGameState(userGame);
                                this.MasterGameState.IsOnlineTournementMode = true;
                                Program.QuickPlay.CurrentGameState = this.MasterGameState;
                            }
                            else
                            {
                                this.SlaveGameState.InitializeGameState(userGame);
                                this.SlaveGameState.IsOnlineTournementMode = true;
                                Program.QuickPlay.CurrentGameState = this.SlaveGameState;

                                FonctionsNatives.rotateCamera(180);
                            }

                            Program.FormManager.CurrentForm = Program.QuickPlay;
                        }
                        else
                        {
                            Program.FormManager.CurrentForm = Program.MainMenu;
                        }

                    }));
            });

            WaitingRoomProxy.On<TournamentEntity>("StartFinal", tournament =>
            {
                Program.OnlineTournament.Invoke(new MethodInvoker(() =>
                {
                    if (tournament.Final.Players[0].Id == User.Instance.UserEntity.Id || tournament.Final.Players[1].Id == User.Instance.UserEntity.Id)
                    {
                        
                        if (tournament.Final.Master.Id == User.Instance.UserEntity.Id)
                        {
                            this.MasterGameState.InitializeGameState(tournament.Final);
                            this.MasterGameState.IsOnlineTournementMode = true;
                            Program.QuickPlay.CurrentGameState = this.MasterGameState;
                        }
                        else
                        {
                            this.SlaveGameState.InitializeGameState(tournament.Final);
                            this.MasterGameState.IsOnlineTournementMode = true;
                            Program.QuickPlay.CurrentGameState = this.SlaveGameState;

                            FonctionsNatives.rotateCamera(180);
                        }

                        Program.FormManager.CurrentForm = Program.QuickPlay;

                    }
                    else
                    {
                        Program.FormManager.CurrentForm = Program.MainMenu;
                    }

                }));
            });

            WaitingRoomProxy.On<TournamentEntity>("TournamentSemiFinalResults", tournament =>
            {
                SemiFinalResultEvent.Invoke(this, new List<UserEntity>() { tournament.SemiFinals[0].Winner, tournament.SemiFinals[1].Winner });
            });

            WaitingRoomProxy.On<TournamentEntity>("TournamentFinalResult", tournament =>
            {
                WinnerResultEvent.Invoke(this, tournament.Final.Winner);
            });
        }


    }
}
