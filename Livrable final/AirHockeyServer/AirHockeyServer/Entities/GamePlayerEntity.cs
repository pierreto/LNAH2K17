﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirHockeyServer.Entities
{
    public class GamePlayerEntity
    {
        public string Username { get; set; }

        public int Id { get; set; }

        public bool IsAi { get; set; }

        public string ProfilePicture { get; set; }

        public GamePlayerEntity(UserEntity user)
        {
            Username = user.Username;
            Id = user.Id;
            IsAi = false;
            ProfilePicture = user.Profile;
        }

        public GamePlayerEntity()
        {

        }

    }
}