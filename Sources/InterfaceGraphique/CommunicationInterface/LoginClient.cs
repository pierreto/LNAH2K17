using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using InterfaceGraphique.Menus;

namespace InterfaceGraphique.CommunicationInterface
{
    class LoginClient
    {
        public static async Task<HttpStatusCode> postLoginAsync(LoginFormMessage loginForm)
        {
            HttpResponseMessage response = await Program.client.PostAsJsonAsync("api/login", loginForm);
            return response.StatusCode;
        }
    }
}
