using AirHockeyServer.DatabaseCore;
using AirHockeyServer.Mapping;

namespace AirHockeyServer.Repositories
{
    public abstract class Repository
    {
        protected DataProvider DataProvider { get; set; }

        protected MapperManager MapperManager { get; set; }

        public Repository(DataProvider dataProvider, MapperManager mapperManager)
        {
            DataProvider = dataProvider;
            MapperManager = mapperManager;
        }
    }
}