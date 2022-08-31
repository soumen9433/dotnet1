using System.Threading.Tasks;
using AutoMapper;
using HazGo.BuildingBlocks.Api.Filters;
using HazGo.BuildingBlocks.Core.Common;
using HazGo.Application.Cities.Commands;
using HazGo.Application.Cities.Dto;
using HazGo.Application.Cities.Queries;
using HazGo.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HazGo.Application.City.Dto;
using HazGo.BuildingBlocks.Persistence.EF.SearchRepository;
using Module = HazGo.BuildingBlocks.Core.Common.Module;

namespace HazGo.Api.Controllers
{
    public class CitiesController : ApiControllerBase
    {
        public CitiesController(IMediator mediator, ILogger<CitiesController> logger, IMapper mapper)
            : base(mediator, logger, mapper)
        {
        }

        [HttpGet]
        [TypeFilter(typeof(AuthorizationAttribute), Arguments = new object[] { Module.City, Rights.View })]
        public async Task<IActionResult> Get(
            [FromQuery(Name = "$filter")] string filter,
            [FromQuery(Name = "$skip")] int skip,
            [FromQuery(Name = "$top")] int top,
            [FromQuery(Name = "$orderby")] string orderBy,
            [FromQuery(Name = "$orderbydirection")] OrderByDirection orderByDirection)
        {
            SearchOptions<City> searchOptions = new SearchOptions<City>()
            {
                Top = top,
                Skip = skip,
                OrderBy = orderBy,
                OrderByDirection = orderByDirection,
                Filter = filter
            };

            var data = await _mediator.Send(new GetCityQuery() { SearchOptions = searchOptions });
            return this.Ok(data);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CityDto))]
        [TypeFilter(typeof(AuthorizationAttribute), Arguments = new object[] { Module.City, Rights.View })]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation(System.DateTime.Now + " CityController @ GetById :  " + id);
            var data = await _mediator.Send(new GetCityByIdQuery() { Id = id });
            if (data == null)
            {
                return this.NotFound("City doesn't exist.");
            }

            _logger.LogInformation(System.DateTime.Now + " CityController @ GetById return Ok");

            return this.Ok(data);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [TypeFilter(typeof(AuthorizationAttribute), Arguments = new object[] { Module.City, Rights.Add })]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateCityResponse))]
        public async Task<IActionResult> Add(CityDtoBase city)
        {
            if (city != null)
            {
                var data = await _mediator.Send(new CreateCityCommand() { CityDto = city });

                return this.Ok(data);
            }
            else
            {
                return this.BadRequest();
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [TypeFilter(typeof(AuthorizationAttribute), Arguments = new object[] { Module.City, Rights.Edit })]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Update(int id, CityDto cityDto)
        {
            if (id > 0)
            {
                var data = await _mediator.Send(new UpdateCityCommand() { Id = id, CityDto = cityDto });
                return this.Ok(data);
            }
            else
            {
                return this.BadRequest();
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [TypeFilter(typeof(AuthorizationAttribute), Arguments = new object[] { Module.City, Rights.Delete })]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Delete(int id)
        {
            if (id > 0)
            {
                var data = await _mediator.Send(new DeleteCityCommand() { Id = id });
                if (!data)
                {
                    return this.BadRequest();
                }
                else
                {
                    return this.Ok(data);
                }
            }
            else
            {
                return this.NotFound();
            }
        }
    }
}
