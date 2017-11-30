using AirHockeyServer.Entities;
using AirHockeyServer.Hubs;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace AirHockeyServer.Services.MatchMaking
{
    public class TournamentMatchMaker : MatchMaker
    {
        public event EventHandler<TournamentEntity> OpponentFound;

        protected Mutex TournamentMutex { get; set; }

        protected List<TournamentEntity> Tournaments { get; set; }

        public ConnectionMapper ConnectionMapper { get; set; }

        public TournamentMatchMaker(ConnectionMapper connectionMapper) : base()
        {
            Tournaments = new List<TournamentEntity>();
            TournamentMutex = new Mutex();
            ConnectionMapper = connectionMapper;
        }

        public override void AddOpponent(List<GamePlayerEntity> players)
        {

            TournamentEntity tournament = new TournamentEntity()
            {
                Id = new Random().Next(),
                State = TournamentState.WaitingForPlayers
            };

            foreach (var player in players)
            {
                tournament.Players.Add(player);
            }

            // add the creator
            AddConnection(tournament.Players.Find(x => !x.IsAi).Id, tournament);

            TournamentMutex.WaitOne();
            Tournaments.Add(tournament);
            TournamentMutex.ReleaseMutex();

            StartPlayersMatching();
        }

        private void AddConnection(int userId, TournamentEntity tournament)
        {
            var connection = ConnectionMapper.GetConnection(userId);
            string tournamentIdString = tournament.Id.ToString();
            GlobalHost.ConnectionManager.GetHubContext<TournamentWaitingRoomHub>().Groups.Add(connection, tournamentIdString).Wait();

            GlobalHost.ConnectionManager.GetHubContext<TournamentWaitingRoomHub>().Clients.Group(tournament.Id.ToString()).OpponentFoundEvent(tournament.Players);

        }

        public override void AddOpponent(GamePlayerEntity player)
        {
            AddOpponent(new List<GamePlayerEntity>() { player });
        }

        ///////////////////////////////////////////////////////////////////////
        ///
        /// @fn static UserEntity GetTournamentOpponents()
        ///
        /// Cette fonction cherche à retourner trois joueurs
        /// en attente de participer à un tournoi
        /// 
        /// @return un ensemble de joueurs
        ///
        ////////////////////////////////////////////////////////////////////////
        protected override void ExecuteMatch()
        {
            TournamentMutex.WaitOne();

            // fusionner avec un autre tournoi si possible
            for (int i = 0; i < Tournaments.Count - 1; i++)
            {
                for (int j = i + 1; j < Tournaments.Count; j++)
                {
                    if (Tournaments[i].Players.Count(x => x.IsAi) == Tournaments[j].Players.Count(x => x.IsAi))
                    {
                        if (Tournaments[i].Players.Count(x => !x.IsAi) + Tournaments[j].Players.Count(x => !x.IsAi) <= 4)
                        {
                            var playersToAdd = Tournaments[j].Players.Where(x => !x.IsAi);
                            Tournaments[i].Players.AddRange(playersToAdd);
                            foreach (var player in playersToAdd)
                            {
                                AddConnection(player.Id, Tournaments[i]);
                                GlobalHost.ConnectionManager.GetHubContext<TournamentWaitingRoomHub>().Clients.Group(Tournaments[i].Id.ToString()).OpponentFoundEvent(Tournaments[i].Players);
                            }
                            Tournaments.Remove(Tournaments[j]);
                            break;
                        }
                    }
                }

            }

            // match waiting players in tournament
            foreach (var tournament in Tournaments)
            {
                if (tournament.Players.Count == 4)
                {
                    OpponentFound.Invoke(this, tournament);
                    Tournaments.Remove(tournament);
                    break;
                }
               
            }

            TournamentMutex.ReleaseMutex();
        }

        public override void RemoveUser(int userId)
        {
            string connection = ConnectionMapper.GetConnection(userId);
            if (connection != null && !string.IsNullOrEmpty(connection))
            {
                var tournament = Tournaments.Find(t => t.Players.Exists(p => p.Id == userId));
                if (tournament != null)
                {
                    GlobalHost.ConnectionManager.GetHubContext<TournamentWaitingRoomHub>().Groups.Remove(connection, tournament.Id.ToString());
                    TournamentMutex.WaitOne();
                    var user = tournament.Players?.Find(x => x.Id == userId);
                    tournament?.Players?.Remove(user);
                    TournamentMutex.ReleaseMutex();
                }
                else
                {
                    WaitingPlayersMutex.WaitOne();
                    _WaitingPlayers = new Queue<GamePlayerEntity>(_WaitingPlayers.Where(x => x.Id != userId));
                    WaitingPlayersMutex.ReleaseMutex();
                }

            }
        }
    }
}