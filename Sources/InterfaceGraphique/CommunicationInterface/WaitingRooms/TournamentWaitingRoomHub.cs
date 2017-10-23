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

        public string Username { get; protected set; }

        protected UserEntity user { get; set; }

        protected HubConnection HubConnection { get; set; }
        protected SlaveGameState SlaveGameState { get; }
        protected MasterGameState MasterGameState { get; }

        public TournamentWaitingRoomHub(SlaveGameState slaveGameState, MasterGameState masterGameState)
        {
            SlaveGameState = slaveGameState;
            MasterGameState = masterGameState;
        }

        public void InitializeHub(HubConnection connection, string username)
        {
            this.HubConnection = connection;
            this.Username = username;
            WaitingRoomProxy = connection.CreateHubProxy("TournamentWaitingRoomHub");

        }

        public void Join()
        {
            UserEntity userToUse;
            if (!test)
            {
                InitializeWaitingRoomEvents();
                test = true;
                Random random = new Random();
                user = new UserEntity
                {
                    UserId = random.Next(),
                    Username = "test"
                };
                userToUse = user;
            }
            else
            {
                Random random = new Random();
                userToUse = new UserEntity
                {
                    UserId = random.Next(),
                    Username = "test"
                };
            }


            WaitingRoomProxy.Invoke("Join", userToUse);
        }

        public void Logout()
        {
            WaitingRoomProxy?.Invoke("Disconnect", this.Username).Wait();
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
            WaitingRoomProxy.Invoke("LeaveTournament", user);
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
                CurrentTournament.SelectedMap = map;
                this.MapUpdatedEvent.Invoke(this, map);
            });
        }

        private void InitializeTournamentsEvents()
        {
            WaitingRoomProxy.On<TournamentEntity>("TournamentStarting", tournament =>
            {
                Program.OnlineTournament.Invoke(new MethodInvoker(() =>
                    {
                        var userGame = tournament.SemiFinals.Find(game => game.Players.Any(player => player.UserId == this.user.UserId));
                        if (userGame != null)
                        {
                            if (userGame.Master.UserId == user.UserId)
                            {
                                this.MasterGameState.InitializeGameState(userGame);
                                this.MasterGameState.IsOnlineTournementMode = true;
                                Program.QuickPlay.CurrentGameState = this.MasterGameState;
                            }
                            else
                            {
                                this.SlaveGameState.InitializeGameState(userGame);
                                Program.QuickPlay.CurrentGameState = this.SlaveGameState;
                            }
                        }

                        Program.FormManager.CurrentForm = Program.QuickPlay;
                    }));

                // start tournament

                //Program.FormManager.CurrentForm = Program.FormManager;
            });

            WaitingRoomProxy.On<TournamentEntity>("StartFinal", tournament =>
            {

                //if (tournament.Final.Players.Contains(user))
                if (true)
                {
                    Program.OnlineTournament.Invoke(new MethodInvoker(() =>
                    {
                        //if (tournament.Final.Master.UserId == user.UserId)
                        if (true)
                        {
                            this.MasterGameState.InitializeGameState(tournament.Final);
                            this.MasterGameState.IsOnlineTournementMode = true;
                            Program.QuickPlay.CurrentGameState = this.MasterGameState;
                        }
                        else
                        {
                            this.SlaveGameState.InitializeGameState(tournament.Final);
                            Program.QuickPlay.CurrentGameState = this.SlaveGameState;
                        }

                        Program.FormManager.CurrentForm = Program.QuickPlay;
                    }));
                }
                else
                {
                    // you lost
                }
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
