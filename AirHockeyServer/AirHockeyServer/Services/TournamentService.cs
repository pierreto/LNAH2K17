using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AirHockeyServer.Entities;
using AirHockeyServer.Services.MatchMaking;

namespace AirHockeyServer.Services
{
    public class TournamentService : ITournamentService
    {
        public void JoinTournament(UserEntity user)
        {
            TournamentMatchMakerService.Instance().AddOpponent(user);
        }

        public void LeaveTournamentWaitingRoom(UserEntity user)
        {
            TournamentMatchMakerService.Instance().RemoveUser(user.Id);
        }

        public TournamentEntity UpdateTournament(TournamentEntity tournamentEntity)
        {
            return tournamentEntity;
        }
    }
}