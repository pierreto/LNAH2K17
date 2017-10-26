using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceGraphique.Entities
{
    public class SignupEntity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public SignupEntity()
        {
            Username = "";
            Password = "";
            ConfirmPassword = "";
        }
    }
}
