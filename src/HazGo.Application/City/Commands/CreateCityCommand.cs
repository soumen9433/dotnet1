using HazGo.Application.City.Dto;
using HazGo.Application.Cities.Dto;
using MediatR;

namespace HazGo.Application.Cities.Commands
{
    public class CreateCityCommand : IRequest<CityDto>
    {
        public CityDtoBase CityDto { get; set; }
    }
}
