using AirHockeyServer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AirHockeyServer.Services.Interfaces
{
    public interface IRankingService
    {
        Task<List<RankingEntity>> GetAllRankings();
    }
}