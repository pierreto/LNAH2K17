using AirHockeyServer.Entities;
using System.Threading.Tasks;

namespace AirHockeyServer.Services
{
    public interface ILoginService
    {
        Task<bool> ValidateCredentials(LoginEntity loginEntity);
        void Logout(LoginEntity message);
    }
}