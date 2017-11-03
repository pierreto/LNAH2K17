using AirHockeyServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirHockeyServer.Manager
{
    public interface ITournamentManager
    {
        void UpdateTournamentState(int tournamentId, GameEntity gameUpdated);

        void AddTournament(TournamentEntity tournament);
    }
}