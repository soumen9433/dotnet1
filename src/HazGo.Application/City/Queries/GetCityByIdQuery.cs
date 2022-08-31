using HazGo.BuildingBlocks.Core.Common;
using HazGo.Application.Cities.Dto;
using MediatR;

namespace HazGo.Application.Cities.Queries
{
    public class GetCityByIdQuery : IRequest<CityDto>
    {
        public int Id { get; set; }
    }
}
