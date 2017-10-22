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

        public static IHubProxy WaitingRoomProxy { get; set; }

        public string Username { get; protected set; }

        protected UserEntity user { get; set; }

        protected HubConnection HubConnection { get; set; }
        protected SlaveGameState SlaveGameState { get; }
        protected MasterGameState MasterGameState { get; }

        //public TournamentWaitingRoomHub(SlaveGameState slaveGameState, MasterGameState masterGameState)
        public TournamentWaitingRoomHub()
        {
            //SlaveGameState = slaveGameState;
            //MasterGameState = masterGameState;
        }

        public void InitializeHub(HubConnection connection, string username)
        {
            this.HubConnection = connection;
            this.Username = username;
            WaitingRoomProxy = connection.CreateHubProxy("TournamentWaitingRoomHub");
            
        }

        public void Join()
        {
            if (!test)
            {
                InitializeEvents();
                test = true;
            }
            Random random = new Random();
            user = new UserEntity
            {
                UserId = random.Next(),
                Username = "test"
            };

            WaitingRoomProxy.Invoke("Join", user);
        }
        private void InitializeEvents()
        {
            WaitingRoomProxy.On<List<UserEntity>>("OpponentFoundEvent", (opponents) =>
            {
                this.OpponentFoundEvent.Invoke(this, opponents);
            });

            WaitingRoomProxy.On<TournamentEntity>("TournamentAllOpponentsFound", (tournament) =>
            {
                this.TournamentAllOpponentsFound.Invoke(this, tournament);
                CurrentTournament = tournament;
                InitializeConfigurationEvents();
            });

            WaitingRoomProxy.On<int>("WaitingRoomRemainingTime", remainingTime =>
            {
                this.RemainingTimeEvent.Invoke(this, remainingTime);
            });
        }

        private void InitializeConfigurationEvents()
        {
            WaitingRoomProxy.On<TournamentEntity>("TournamentStarting", tournament =>
            {
                if(tournament.State == TournamentState.SemiFinals)
                {
                    Program.LobbyHost.Invoke(new MethodInvoker(() =>
                    {
                        //Program.QuickPlay.CurrentGameState.IsOnlineTournementMode = true;
                        //if (tournament.SemiFinals.Any(game => game.Master.UserId == this.user.UserId))
                        //{
                        //this.MasterGameState.InitializeGameState(tournament.SemiFinals.Find(game => game.Master.UserId == this.user.UserId));
                        //Program.QuickPlay.CurrentGameState.IsTournementMode = true;
                        //this.MasterGameState.InitializeGameState(tournament.SemiFinals[0]);
                        //        Program.QuickPlay.CurrentGameState = this.MasterGameState;
                        FonctionsNatives.setCurrentOpponentType((int)OpponentType.VIRTUAL_PLAYER);

                        PlayerProfile selectedProfile = Program.ConfigurationMenu.GetProfile(Program.TournementMenu.Player1Profile);
                        FonctionsNatives.aiActiveProfile(selectedProfile.Speed, selectedProfile.Passivity);

                        StringBuilder player1Name = new StringBuilder(4);
                        StringBuilder player2Name = new StringBuilder(4);
                        player1Name.Append("tata");
                        player2Name.Append("papa");
                        FonctionsNatives.setPlayerNames(player1Name, player2Name);

                        float[] player1Color = new float[4] { Color.Red.R, Color.Red.G, Color.Red.B, Color.Red.A };
                        float[] player2Color = new float[4] { Color.Blue.R, Color.Blue.G, Color.Blue.B, Color.Blue.A };
                        FonctionsNatives.setPlayerColors(player1Color, player2Color);

                        Program.FormManager.CurrentForm = Program.QuickPlay;
                        //}
                        //else
                        //{
                        //    this.SlaveGameState.InitializeGameState(tournament.SemiFinals.Find(game => game.Slave.UserId == this.user.UserId));

                        //    Program.QuickPlay.CurrentGameState = this.SlaveGameState;
                        //    Program.FormManager.CurrentForm = Program.QuickPlay;

                        //    FonctionsNatives.rotateCamera(180);
                        //}
                    }));

                }
            });

            WaitingRoomProxy.On<MapEntity>("TournamentMapUpdatedEvent", map =>
            {
                CurrentTournament.SelectedMap = map;
                this.MapUpdatedEvent.Invoke(this, map);

            });
            
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

    }
}
