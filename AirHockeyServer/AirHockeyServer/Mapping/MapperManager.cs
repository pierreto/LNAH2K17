using AirHockeyServer.DatabaseCore;
using AirHockeyServer.Entities;
using AirHockeyServer.Pocos;
using AirHockeyServer.Repositories;
using AirHockeyServer.Services.Interfaces;
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
                // À faire pour chaque pair poco/entity:
                //Not necessary if same attribute names from poco to entity
                //cfg.CreateMap<UserPoco, UserEntity>()
                //.ForMember(
                //    dest => dest.Id,
                //    opt => opt.MapFrom(src => src.Id)
                //);

                cfg.CreateMap<UserPoco, UserEntity>();
                cfg.CreateMap<UserEntity, UserPoco>();
                cfg.CreateMap<PasswordPoco, PasswordEntity>();
                cfg.CreateMap<PasswordEntity, PasswordPoco>();

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

                cfg.CreateMap<FriendPoco, FriendRequestEntity>()
                .ForMember(
                    dest => dest.Requestor,
                    opt => opt.MapFrom(src => new UserEntity { Id = src.RequestorID, Username = src.Requestor.Username }))
                .ForMember(
                    dest => dest.Friend,
                    opt => opt.MapFrom(src => new UserEntity { Id = src.FriendID, Username = src.Friend.Username }));

                cfg.CreateMap<FriendRequestEntity, FriendPoco>()
                .ForMember(
                    dest => dest.RequestorID,
                    opt => opt.MapFrom(src => src.Requestor.Id))
                .ForMember(
                    dest => dest.FriendID,
                    opt => opt.MapFrom(src => src.Friend.Id))
                .ForMember(
                    dest => dest.Requestor,
                    opt => opt.MapFrom(src => new UserPoco { Id = src.Requestor.Id, Username = src.Requestor.Username }))
                .ForMember(
                    dest => dest.Friend,
                    opt => opt.MapFrom(src => new UserPoco { Id = src.Friend.Id, Username = src.Friend.Username }));

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
                        opt => opt.MapFrom(src => src.Winner.Id))
                    .ForMember(
                        dest => dest.Player1,
                        opt => opt.MapFrom(src => src.Players[0]))
                    .ForMember(
                        dest => dest.Player2,
                        opt => opt.MapFrom(src => src.Players[1]));

                cfg.CreateMap<TournamentEntity, TournamentPoco>()
                    .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                    .ForMember(
                    dest => dest.PlayedMap,
                    opt => opt.MapFrom(src => src.SelectedMap))
                    .ForMember(
                    dest => dest.Winner,
                    opt => opt.MapFrom(src => src.Winner.Id))
                    .ForMember(
                    dest => dest.Player1,
                    opt => opt.MapFrom(src => src.Players[0]))
                    .ForMember(
                    dest => dest.Player2,
                    opt => opt.MapFrom(src => src.Players[1]))
                    .ForMember(
                    dest => dest.Player3,
                    opt => opt.MapFrom(src => src.Players[2]))
                    .ForMember(
                    dest => dest.Player4,
                    opt => opt.MapFrom(src => src.Players[3]));
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
