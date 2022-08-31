using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HazGo.Application.City.Mapper;
using HazGo.Application.Cities.Dto;
using HazGo.BuildingBlocks.Core.Common;
using HazGo.BuildingBlocks.Core.Domain;
using HazGo.Domain.Entities;
using HazGo.Domain.Repositories;
using MediatR;

namespace HazGo.Application.Cities.Commands
{
    public class CityCommandHandler : CityBaseHandler, IRequestHandler<CreateCityCommand, CityDto>,
        IRequestHandler<UpdateCityCommand, bool>, IRequestHandler<DeleteCityCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICityRepository _cityRepository;
        private readonly IMapper _mapper;

        public CityCommandHandler(IUnitOfWork unitOfWork, ICityRepository cityRepository, IMapper mapper)
            : base(unitOfWork, cityRepository, mapper)
        {
            this._unitOfWork = unitOfWork;
            this._cityRepository = cityRepository;
            this._mapper = mapper;
        }

        public async Task<CityDto> Handle(CreateCityCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<HazGo.Domain.Entities.City>(request.CityDto);
            
            _cityRepository.Add(entity);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<CityDto>(entity);
        }

        public async Task<bool> Handle(UpdateCityCommand request, CancellationToken cancellationToken)
        {
            HazGo.Domain.Entities.City entity = await _cityRepository.GetByIdAsync(request.Id);

            entity = CityMapper.Map(entity, request.CityDto);
            _cityRepository.Update(entity);
            await _unitOfWork.CommitAsync();

            return true;
        }

        public async Task<bool> Handle(DeleteCityCommand request, CancellationToken cancellationToken)
        {
            HazGo.Domain.Entities.City entity = await _cityRepository.GetByIdAsync(request.Id);
            _cityRepository.Delete(entity);
            await this._unitOfWork.CommitAsync(cancellationToken);

            return true;
        }
    }
}
