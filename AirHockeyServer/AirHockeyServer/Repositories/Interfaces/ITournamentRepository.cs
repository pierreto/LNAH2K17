using AirHockeyServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AirHockeyServer.Repositories.Interfaces
{
    public interface ITournamentRepository
    {
        Task<TournamentEntity> CreateTournament(TournamentEntity tournament);
    }
}