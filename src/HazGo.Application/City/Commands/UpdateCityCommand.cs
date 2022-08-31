using HazGo.BuildingBlocks.Core.Common;
using HazGo.Application.Cities.Dto;
using MediatR;

namespace HazGo.Application.Cities.Commands
{
    public class UpdateCityCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public CityDto CityDto { get; set; }
    }
}
