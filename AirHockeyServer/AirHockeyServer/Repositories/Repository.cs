using AirHockeyServer.Mapping;

namespace AirHockeyServer.Repositories
{
    public abstract class Repository
    {
        protected MapperManager MapperManager { get; set; }

        public Repository(MapperManager mapperManager)
        {
            MapperManager = mapperManager;
        }
    }
}