using AirHockeyServer.Entities;
using AirHockeyServer.Mapping;
using AirHockeyServer.Pocos;
using AirHockeyServer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AirHockeyServer.Repositories
{
    public class TournamentRepository : Repository, ITournamentRepository
    {
        protected IMapRepository MapRepository { get; private set; }

        public TournamentRepository(MapperManager mapperManager, IMapRepository mapRepository)
            :base(mapperManager)
        {
            MapRepository = mapRepository;
        }

        public async Task<TournamentEntity> CreateTournament(TournamentEntity tournament)
        {
            try
            {
                using (MyDataContext DC = new MyDataContext())
                {
                    TournamentPoco tournamentToCreate = MapperManager.Map<TournamentEntity, TournamentPoco>(tournament);
                    DC.GetTable<TournamentPoco>().InsertOnSubmit(tournamentToCreate);

                    await Task.Run(() => DC.SubmitChanges());

                    return await GetTournament(tournament.Id);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[TournamentRepository.CreateTournament] " + e.ToString());
                return null;
            }
        }

        public async Task<TournamentEntity> GetTournament(int tournamentId)
        {
            try
            {
                using (MyDataContext DC = new MyDataContext())
                {
                    IQueryable<TournamentPoco> queryable =
                    from tournaments in DC.GetTable<TournamentPoco>() where tournaments.Id == tournamentId select tournaments;

                    var results = await Task<IEnumerable<TournamentPoco>>.Run(
                        () => queryable.ToArray());

                    TournamentPoco poco = results.Length > 0 ? results.First() : null;

                    TournamentEntity result = new TournamentEntity();
                    IEnumerable<MapEntity> maps = await MapRepository.GetMaps();
                    // TODO : UPDATE WHEN MAP DONE
                    result.SelectedMap = maps.First();
                    // TODO GET USER
                    result.Winner = new UserEntity
                    {
                        Id = poco.Winner
                    };
                    result.Players[0] = new UserEntity
                    {
                        Id = poco.Player1
                    };
                    result.Players[1] = new UserEntity
                    {
                        Id = poco.Player2
                    };
                    result.Players[2] = new UserEntity
                    {
                        Id = poco.Player3
                    };
                    result.Players[3] = new UserEntity
                    {
                        Id = poco.Player4
                    };

                    result.Id = poco.Id;
                    return result;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[TournamentRepository.GetTournament] " + e.ToString());
                return null;
            }
        }

        public async Task<int> GetUserTournamentsNb(int userId)
        {
            try
            {
                using (MyDataContext DC = new MyDataContext())
                {
                    IQueryable<TournamentPoco> queryable =
                    from tournaments in DC.GetTable<TournamentPoco>()
                    where
                        tournaments.Player1 == userId ||
                        tournaments.Player2 == userId ||
                        tournaments.Player3 == userId ||
                        tournaments.Player4 == userId
                    select tournaments;

                    var results = await Task<IEnumerable<TournamentPoco>>.Run(
                        () => queryable.ToArray());

                    return results.Length;
                }
                
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[TournamentRepository.GetUserTournamentsNb] " + e.ToString());
                return 0;
            }
        }
    }
}