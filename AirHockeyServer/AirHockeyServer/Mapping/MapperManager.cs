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
                // À faire pour chaque pair poco/entity:

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