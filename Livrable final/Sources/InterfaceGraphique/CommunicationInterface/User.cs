﻿using InterfaceGraphique.Entities;
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

        public bool IsConnected { get; set; }

        public List<StoreItemEntity> Inventory { get; set; }

        public PlayerStatsEntity Stats { get; set; }

        private static User instance;

        public User()
        {
            UserEntity = new UserEntity()
            {
                Email = "local",
                Username = "local",
                Id = -1,
                Name = "local"

            };
        }

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