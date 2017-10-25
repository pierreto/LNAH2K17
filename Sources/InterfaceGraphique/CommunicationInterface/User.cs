using InterfaceGraphique.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace InterfaceGraphique.CommunicationInterface
{
    public class User
    {
        public UserEntity UserEntity { get; set; }
        private static User instance;
        public static User Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new User();
                }
                return instance;
            }
        }
    }
}