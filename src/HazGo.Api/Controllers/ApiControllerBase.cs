using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HazGo.Api.Controllers
{
    /// <summary>
    /// ApiControllerBase.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected readonly IMapper _mapper;
        protected readonly ILogger _logger;
        protected readonly IMediator _mediator;

        protected ApiControllerBase(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected ApiControllerBase(IMediator mediator, ILogger logger, IMapper mapper)
        {
            _mediator = mediator;
            _logger = logger;
            _mapper = mapper;
        }

        protected ApiControllerBase(IMediator mediator, ILogger logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        protected ApiControllerBase(IMapper mapper, ILogger logger)
        {
            _mapper = mapper;
            _logger = logger;
        }
    }
}
