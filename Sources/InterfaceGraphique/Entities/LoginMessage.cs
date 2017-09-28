using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceGraphique.Entities
{
    class LoginMessage
    {
        public string username { get; set; }
        public string password { get; set; }

        public LoginMessage()
        { }

        /*
        public LoginMessage(LoginFormMessage loginForm)
        {
            this.username = loginForm.LoginName;
            this.password = loginForm.Password;
        }
        */
    }
}
