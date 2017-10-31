using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirHockeyServer.Events.EventManagers
{
    public class GameManager
    {
        private static GameManagerInstance _instance;
        
        protected GameManager()
        {
        }

        public static GameManagerInstance Instance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            if (_instance == null)
            {
                _instance = new GameManagerInstance();
            }

            return _instance;
        }
    }
}