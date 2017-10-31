using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirHockeyServer.Services.MatchMaking
{
    public class GameMatchMakerService
    {
        private static GameMatchMaker _instance;

        // Constructor is 'protected'
        protected GameMatchMakerService()
        {
        }

        public static GameMatchMaker Instance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            if (_instance == null)
            {
                _instance = new GameMatchMaker();
            }

            return _instance;
        }
    }
}