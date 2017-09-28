using AirHockeyServer.Entities;

namespace AirHockeyServer.Services
{
    public interface ILoginService
    {
        void login(LoginMessage message);
        void disconnect();
    }
}