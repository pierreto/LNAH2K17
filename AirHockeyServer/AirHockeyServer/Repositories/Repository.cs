using AirHockeyServer.DatabaseCore;
using AirHockeyServer.Mapping;

namespace AirHockeyServer.Repositories
{
    public abstract class Repository<T> where T : class, new()
    {

        protected DataProvider DataProvider { get; set; }
        protected MapperManager MapperManager { get; set; }

        public Repository()
        {
            DataProvider = new DataProvider();
            MapperManager = new MapperManager();
        }
    }
}