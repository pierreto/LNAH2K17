using AirHockeyServer.Entities;

namespace AirHockeyServer.Services
{
    public interface ILoginService
    {
        void login(LoginFormMessage message);
        void disconnect();
    }
}