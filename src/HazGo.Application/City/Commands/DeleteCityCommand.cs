using HazGo.BuildingBlocks.Core.Common;
using MediatR;

namespace HazGo.Application.Cities.Commands
{
    public class DeleteCityCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
