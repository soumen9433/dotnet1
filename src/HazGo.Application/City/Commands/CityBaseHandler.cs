namespace HazGo.Application.Cities.Commands
{
    using AutoMapper;
    using HazGo.BuildingBlocks.Core.Domain;
    using HazGo.Domain.Repositories;

    public abstract class CityBaseHandler
    {
        protected CityBaseHandler(IUnitOfWork unitOfWork, ICityRepository cityRepository, IMapper mapper)
        {
        }
    }
}
