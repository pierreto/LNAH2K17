using AirHockeyServer.Entities;
using System.Threading.Tasks;

namespace AirHockeyServer.Services.Interfaces
{
    public interface IProfileService
    {
        Task<ProfileEntity> GetProfileById(int id);
        Task<ProfileEntity> GetProfileByUsername(string username);
    }
}