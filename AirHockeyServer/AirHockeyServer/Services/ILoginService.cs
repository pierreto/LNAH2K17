using AirHockeyServer.Entities;
using System.Threading.Tasks;

namespace AirHockeyServer.Services
{
    public interface ILoginService
    {
        Task<bool> ValidateCredentials(LoginEntity message);
        void Logout(LoginEntity message);
    }
}