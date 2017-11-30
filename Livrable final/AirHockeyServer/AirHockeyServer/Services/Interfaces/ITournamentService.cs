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
        void JoinTournament(List<GamePlayerEntity> players);

        void UpdateTournament(int tournamentId, MapEntity selectedMap);
        
        Task LeaveTournament(int userId);
    }
}