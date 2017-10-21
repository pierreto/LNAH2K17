using AirHockeyServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace AirHockeyServer.Services
{
    public interface ITournamentService
    {
        void JoinTournament(UserEntity user);

        TournamentEntity UpdateTournament(TournamentEntity tournamentEntity);

        void LeaveTournament(UserEntity user);
    }
}