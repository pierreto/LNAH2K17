﻿using AirHockeyServer.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.Unity;

namespace AirHockeyServer.Services.MatchMaking
{
    public class TournamentMatchMakerService
    {
        private static TournamentMatchMaker _instance;

        // Constructor is 'protected'
        protected TournamentMatchMakerService()
        {
        }

        public static TournamentMatchMaker Instance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            if (_instance == null)
            {
                _instance = new TournamentMatchMaker(WebApiApplication.UnityContainer.Resolve<ConnectionMapper>());
            }

            return _instance;
        }
    }
}