using AutoMapper;
using HazGo.Application.City.Dto;
using HazGo.Application.Cities.Dto;
using HazGo.Domain.Entities;

namespace HazGo.Application
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CityDto, HazGo.Domain.Entities.City>().ReverseMap();
            CreateMap<CityDtoBase, HazGo.Domain.Entities.City>().ReverseMap();
        }
    }
}
