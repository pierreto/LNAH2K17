using AirHockeyServer.Entities;
using System.Threading.Tasks;

namespace AirHockeyServer.Services
{
    public interface ISignupService
    {
        Task Signup(SignupEntity signupEntity);
    }
}