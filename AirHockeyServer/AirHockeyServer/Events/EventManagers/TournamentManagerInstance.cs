using AirHockeyServer.Entities;
using AirHockeyServer.Hubs;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Timers;

namespace AirHockeyServer.Events.EventManagers
{
    public class TournamentManagerInstance
    {
        private const int FINAL_DELAI = 10000;

        protected Dictionary<int, TournamentEntity> Tournaments { get; set; }

        protected Dictionary<int, int> ElapsedTime { get; set; }

        public TournamentManagerInstance()
        {
            this.Tournaments = new Dictionary<int, TournamentEntity>();
            this.ElapsedTime = new Dictionary<int, int>();
        }

        public void AddTournament(TournamentEntity tournament)
        {
            if (!Tournaments.ContainsKey(tournament.Id))
            {
                Tournaments[tournament.Id] = tournament;

                GameManager.Instance().AddGame(tournament.SemiFinals[0]);
                GameManager.Instance().AddGame(tournament.SemiFinals[1]);
            }
        }

        public void UpdateTournamentState(int tournamentId, GameEntity gameUpdated)
        {
            UpdateTournamentGames(gameUpdated, tournamentId);
            if (Tournaments.ContainsKey(tournamentId))
            {
                var tournament = Tournaments[tournamentId];
                var hub = GlobalHost.ConnectionManager.GetHubContext<TournamentWaitingRoomHub>();

                if (tournament.SemiFinals.All(game => game.GameState == GameState.Ended))
                {
                    if (tournament.Final?.GameState == GameState.Ended)
                    {
                        // end of tournament
                        tournament.State = TournamentState.Done;

                        hub.Clients.Group(tournament.Id.ToString()).TournamentFinalResult(tournament);

                        // save to DB

                        Tournaments.Remove(tournamentId);
                    }
                    else
                    {
                        // do final
                        GameEntity finalGame = new GameEntity
                        {
                            Players = new UserEntity[] { tournament.SemiFinals[0].Winner, tournament.SemiFinals[1].Winner },
                            GameState = GameState.InProgress,
                            GameId = new Random().Next(),
                            CreationDate = DateTime.Now,
                            TournamentId = tournament.Id
                        };
                        finalGame.Master = finalGame.Players[0];
                        finalGame.Slave = finalGame.Players[1];

                        tournament.State = TournamentState.Final;
                        tournament.Final = finalGame;
                        

                        GameManager.Instance().AddGame(finalGame);
                        Tournaments[tournament.Id] = tournament;

                        hub.Clients.Group(tournament.Id.ToString()).TournamentSemiFinalResults(tournament);

                        Timer timer = new Timer();
                        timer.Interval = 1000;
                        timer.Elapsed += (timerSender, e) => FinalCountdown(timerSender, e, tournament, timer);

                        this.ElapsedTime.Add(tournament.Id, 0);

                        timer.Start();
                    }
                }
                else
                {
                    hub.Clients.Group(tournament.Id.ToString()).TournamentSemiFinalResults(tournament);
                    //testFlow(tournament.Id);
                }

                Tournaments[tournament.Id] = tournament;
            }
        }

        private void testFlow(int tournamentId)
        {
            var hub = GlobalHost.ConnectionManager.GetHubContext<TournamentWaitingRoomHub>();
            var tournament = Tournaments[tournamentId];

            tournament.SemiFinals[1].Winner = tournament.SemiFinals[1].Players[0];
            tournament.SemiFinals[1].GameState = GameState.Ended;

            GameEntity finalGame = new GameEntity
            {
                Players = new UserEntity[] { tournament.SemiFinals[0].Winner, tournament.SemiFinals[1].Players[0] },
                GameState = GameState.InProgress,
                GameId = new Random().Next(),
                CreationDate = DateTime.Now,
                TournamentId = tournament.Id
            };
            finalGame.Master = finalGame.Players[0];
            finalGame.Slave = finalGame.Players[1];

            GameManager.Instance().AddGame(finalGame);

            tournament.State = TournamentState.Final;
            tournament.Final = finalGame;
            
            Tournaments[tournament.Id] = tournament;

            hub.Clients.Group(tournament.Id.ToString()).TournamentSemiFinalResults(tournament);

            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += (timerSender, e) => FinalCountdown(timerSender, e, tournament, timer);

            this.ElapsedTime.Add(tournament.Id, 0);

            timer.Start();
        }

        private void UpdateTournamentGames(GameEntity gameUpdated, int tournamentId)
        {
            if (Tournaments.ContainsKey(tournamentId))
            {
                for (int i = 0; i < 2; i++)
                {
                    if (Tournaments[tournamentId].SemiFinals[i].GameId == gameUpdated.GameId)
                    {
                        Tournaments[tournamentId].SemiFinals[i] = gameUpdated;
                        break;
                    }
                }

                if (Tournaments[tournamentId].Final?.GameId == gameUpdated.GameId)
                {
                    Tournaments[tournamentId].Final = gameUpdated;
                }
            }
        }

        private void FinalCountdown(object timerSender, ElapsedEventArgs e, TournamentEntity tournament, Timer timer)
        {
            this.ElapsedTime[tournament.Id] += 1000;
            if (ElapsedTime[tournament.Id] >= FINAL_DELAI)
            {
                timer.Stop();
                ElapsedTime.Remove(tournament.Id);

                var hub = GlobalHost.ConnectionManager.GetHubContext<TournamentWaitingRoomHub>();
                hub.Clients.Group(tournament.Id.ToString()).StartFinal(tournament);
            }
        }
    }
}