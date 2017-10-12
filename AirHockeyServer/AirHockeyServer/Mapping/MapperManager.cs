using AirHockeyServer.Entities;
using AirHockeyServer.Pocos;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirHockeyServer.Mapping
{
    public class MapperManager
    {
        public IMapper Mapper { get; private set; } 

        public MapperManager()
        {
            var config = new MapperConfiguration(cfg =>
            {
                // A faire pour chaque pair poco/entity
                cfg.CreateMap<UserPoco, UserEntity>()
                .ForMember(
                    dest => dest.UserId,
                    opt => opt.MapFrom(src => src.UserId));

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
            });

            Mapper = config.CreateMapper();
        }

        public TDest Map<TSource, TDest>(TSource source)
        {
            return Mapper.Map<TSource, TDest>(source);
        }
    }
}