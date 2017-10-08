using AirHockeyServer.DatabaseCore;
using AirHockeyServer.Entities;
using AirHockeyServer.Pocos;
using AirHockeyServer.Repositories;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirHockeyServer.Mapping
{
    public class MapperManager
    {
        public IMapper Mapper { get; set; }

        public UserRepository UserRepository;
        public DataProvider DataProvider;

        public MapperManager()
        {
            var config = new MapperConfiguration(cfg =>
            {
                // A faire pour chaque pair poco/entity
                cfg.CreateMap<PasswordPoco, PasswordEntity>();
                cfg.CreateMap<PasswordEntity, PasswordPoco>();
                cfg.CreateMap<UserPoco, UserEntity>();
                cfg.CreateMap<UserEntity, UserPoco>();
                //Not necessary if same attribute names from poco to entity
                //cfg.CreateMap<UserPoco, UserEntity>()
                //.ForMember(
                //    dest => dest.Id,
                //    opt => opt.MapFrom(src => src.Id)
                //);
            });

            config.AssertConfigurationIsValid();
            Mapper = config.CreateMapper();
        }

        //public T Map<K, T>(K poco) where T : Entity where K : Poco
        //{
        //    return Mapper.Map<K, T>(poco);
        //}

        //public K Map<K, T>(T entity) where T : Entity where K : Poco
        //{
        //    return Mapper.Map<T, K>(entity);
        //}
    }
}