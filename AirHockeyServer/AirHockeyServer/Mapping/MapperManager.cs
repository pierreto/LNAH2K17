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

                cfg.CreateMap<MapPoco, MapEntity>()
                .ForMember(
                    dest => dest.LastBackup,
                    opt => opt.MapFrom(src => src.CreationDate))
                .ForMember(
                    dest => dest.MapName,
                    opt => opt.MapFrom(src => src.Name));

                cfg.CreateMap<MapEntity, MapPoco>()
                .ForMember(
                    dest => dest.CreationDate,
                    opt => opt.MapFrom(src => src.LastBackup))
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => src.MapName));

                cfg.CreateMap<PasswordPoco, PasswordEntity>();
                cfg.CreateMap<PasswordEntity, PasswordPoco>();
                cfg.CreateMap<UserPoco, UserEntity>();
                cfg.CreateMap<UserEntity, UserPoco>();

                cfg.CreateMap<StatsEntity, StatsPoco>();
                cfg.CreateMap<StatsPoco, StatsEntity>();

                cfg.CreateMap<GameEntity, GamePoco>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => src.GameId))
                .ForMember(
                    dest => dest.PlayedMap,
                    opt => opt.MapFrom(src => src.SelectedMap.Id))
                .ForMember(
                    dest => dest.Winner,
                    opt => opt.MapFrom(src => src.Winner.Id));

                //Not necessary if same attribute names from poco to entity
                //cfg.CreateMap<UserPoco, UserEntity>()
                //.ForMember(
                //    dest => dest.Id,
                //    opt => opt.MapFrom(src => src.Id)
                //);
            });

            //config.AssertConfigurationIsValid();
            Mapper = config.CreateMapper();
        }

        public TDest Map<TSource, TDest>(TSource source)
        {
            return Mapper.Map<TSource, TDest>(source);
        }
    }
}