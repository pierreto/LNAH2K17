using AirHockeyServer.Entities;
using System.Threading.Tasks;

namespace AirHockeyServer.Services.Interfaces
{
    public interface ISignupService
    {
        Task<int> Signup(SignupEntity signupEntity);
    }
}