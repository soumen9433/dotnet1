using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HazGo.Application.Cities.Dto;
using HazGo.BuildingBlocks.Persistence.EF.SearchRepository;
using HazGo.Domain.Entities;
using MediatR;

namespace HazGo.Application.Cities.Queries
{
    public class GetCityQuery : IRequest<SearchResult<CityDto>>
    {
        public SearchOptions<HazGo.Domain.Entities.City> SearchOptions { get; set; }
    }
}
