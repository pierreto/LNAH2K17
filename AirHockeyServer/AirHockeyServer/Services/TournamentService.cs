using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AirHockeyServer.Entities;
using AirHockeyServer.Services.MatchMaking;
using AirHockeyServer.Services.Interfaces;
using AirHockeyServer.Events.EventManagers;

namespace AirHockeyServer.Services
{
    public class TournamentService : ITournamentService
    {
        public TournamentService(TournamentWaitingRoomEventManager tournamentWaitingRoomEVentManager)
        {
            TournamentWaitingRoomEVentManager = tournamentWaitingRoomEVentManager;
        }

        public TournamentWaitingRoomEventManager TournamentWaitingRoomEVentManager { get; set; }

        public void JoinTournament(List<GamePlayerEntity> players)
        {
            TournamentMatchMakerService.Instance().AddOpponent(players);
        }

        public void LeaveTournamentWaitingRoom(GamePlayerEntity user, int tournamentId)
        {
            TournamentMatchMakerService.Instance().RemoveUser(user.Id);
            if(tournamentId == 0)
            {
                // the user is waiting for players
                TournamentWaitingRoomEVentManager.RemoveUser(user.Id);
            }
        }

        public void UpdateTournament(int tournamentId, MapEntity map)
        {
            TournamentWaitingRoomEVentManager.SetMap(tournamentId, map);
        }
    }
}