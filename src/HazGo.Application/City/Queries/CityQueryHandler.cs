using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HazGo.Application.Cities.Dto;
using HazGo.Domain.Entities;
using HazGo.Domain.Repositories;
using MediatR;
using EntityStatus = HazGo.Domain.Entities.EntityStatus;
using EnumExtension = HazGo.BuildingBlocks.Common.Extensions.EnumExtension;
using HazGo.BuildingBlocks.Persistence.EF.SearchRepository;
using System.Collections.Generic;

namespace HazGo.Application.Cities.Queries
{
    public class CityQueryHandler : IRequestHandler<GetCityByIdQuery, CityDto>,
        IRequestHandler<GetCityQuery, SearchResult<CityDto>>
    {
        private const int Validityminutes = 30;
        private readonly ICityRepository _cityRepository;
        private readonly IMapper _mapper;


        public CityQueryHandler(ICityRepository cityRepository, IMapper mapper)
        {
            _cityRepository = cityRepository;
            _mapper = mapper;
        }

        public async Task<CityDto> Handle(GetCityByIdQuery request, CancellationToken cancellationToken)
        {
            HazGo.Domain.Entities.City entity = await _cityRepository.GetByIdAsync(request.Id);
            if (entity != null)
            {
                return this._mapper.Map<CityDto>(entity);
            }
            else
            {
                return null;
            }
        }

        public async Task<SearchResult<CityDto>> Handle(GetCityQuery request, CancellationToken cancellationToken)
        {
            IQueryable<HazGo.Domain.Entities.City> query = _cityRepository.SearchQueryAsync();
            (IList<HazGo.Domain.Entities.City>? values, int totalCount) = await request.SearchOptions.SearchAsync(query);

            List<CityDto> result = _mapper.Map<List<CityDto>>(values);

            return new SearchResult<CityDto>(result, totalCount);
        }
    }
}
