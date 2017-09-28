using AirHockeyServer.Entities;

namespace AirHockeyServer.Services
{
    public interface ILoginService
    {
        void Login(LoginMessage message);
        void Logout(LoginMessage message);
    }
}