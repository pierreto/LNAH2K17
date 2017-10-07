using AirHockeyServer.DatabaseCore;
using AirHockeyServer.Mapping;

namespace AirHockeyServer.Repositories
{
    public abstract class Repository<T> where T : class, new()
    {

        protected static DataProvider DataProvider { get; set; }
        protected static MapperManager MapperManager { get; set; }

        private static T _instance = null;

        public static T Instance()
        {
            if (_instance == null)
                _instance = new T();
            return _instance;
        }


        public Repository()
        {
            DataProvider = new DataProvider();
            MapperManager = new MapperManager();
        }
    }
}