using AirHockeyServer.Entities;
using AirHockeyServer.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AirHockeyServer.Services
{
    public class PasswordService
    {
        private PasswordRepository PasswordRepository;

        public PasswordService()
        {
            PasswordRepository = new PasswordRepository();
        }

        public async Task<PasswordEntity> GetPasswordById(int id)
        {
            return await PasswordRepository.GetPasswordById(id);
        }

        /*
        public async Task PostUser()
        {
            // TODO
        }
        */
    }
}