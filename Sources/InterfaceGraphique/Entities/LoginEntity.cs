﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceGraphique.Entities
{
    public class LoginEntity
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public LoginEntity()
        {
            Username = "";
            Password = "";
        }
    }
}
