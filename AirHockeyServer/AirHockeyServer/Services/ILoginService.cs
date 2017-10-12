using AirHockeyServer.Entities;

namespace AirHockeyServer.Services
{
    public interface ILoginService
    {
        void Login(LoginEntity message);
        void Logout(LoginEntity message);
    }
}