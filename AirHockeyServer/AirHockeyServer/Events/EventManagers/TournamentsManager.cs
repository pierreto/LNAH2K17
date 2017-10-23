using AirHockeyServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirHockeyServer.Events.EventManagers
{
    public class TournamentsManager
    {
        private static TournamentManagerInstance _instance;

        protected TournamentsManager()
        {
        }

        public static TournamentManagerInstance Instance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            if (_instance == null)
            {
                _instance = new TournamentManagerInstance();
            }

            return _instance;
        }
    }
}