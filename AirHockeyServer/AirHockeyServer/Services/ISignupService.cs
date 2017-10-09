using AirHockeyServer.Entities;

namespace AirHockeyServer.Services
{
    public interface ISignupService
    {
        void Signup(SignupEntity message);
    }
}