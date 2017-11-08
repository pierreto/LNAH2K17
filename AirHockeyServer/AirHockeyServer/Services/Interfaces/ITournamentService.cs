using AirHockeyServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace AirHockeyServer.Services.Interfaces
{
    public interface ITournamentService
    {
        void JoinTournament(UserEntity user);

        void UpdateTournament(int tournamentId, MapEntity selectedMap);

        void LeaveTournamentWaitingRoom(UserEntity user, int tournamentId);
    }
}