using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AirHockeyServer.Entities;
using AirHockeyServer.Services.MatchMaking;
using AirHockeyServer.Services.Interfaces;
using AirHockeyServer.Events.EventManagers;
using AirHockeyServer.Manager;

namespace AirHockeyServer.Services
{
    public class TournamentService : ITournamentService
    {
        public TournamentService(TournamentWaitingRoomEventManager tournamentWaitingRoomEVentManager,
            IPlayOnlineManager playOnlineManager)
        {
            TournamentWaitingRoomEVentManager = tournamentWaitingRoomEVentManager;
            PlayOnlineManager = playOnlineManager;
        }

        public TournamentWaitingRoomEventManager TournamentWaitingRoomEVentManager { get; set; }
        public IPlayOnlineManager PlayOnlineManager { get; }

        public void JoinTournament(List<GamePlayerEntity> players)
        {
            TournamentMatchMakerService.Instance().AddOpponent(players);
        }

        public async Task LeaveTournament(int userId)
        {
            TournamentMatchMakerService.Instance().RemoveUser(userId);
            TournamentWaitingRoomEventManager.LeaveTournament();
            await PlayOnlineManager.PlayerLeaveLiveGame(userId);
            await PlayOnlineManager.PlayerLeaveLiveTournament(userId);
        }

        public void UpdateTournament(int tournamentId, MapEntity map)
        {
            TournamentWaitingRoomEVentManager.SetMap(tournamentId, map);
        }
    }
}