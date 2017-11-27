using InterfaceGraphique.CommunicationInterface.RestInterface;
using InterfaceGraphique.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceGraphique.Services
{
    public class UserService : Service
    {
        public async Task<List<UserEntity>> GetAllUsers()
        {
            try
            {
                HttpResponseMessage response = await Program.client.GetAsync("api/user");
                return await HttpResponseParser.ParseResponse<List<UserEntity>>(response);
            }
            catch (Exception)
            {
                await OnException();
                return null;
            }
        }
    }
}
