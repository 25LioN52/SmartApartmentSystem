using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Queries;

namespace SmartApartmentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StatusController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<ModuleStatus>>> GetStatuses()
        {
            var result = await _mediator.Send(new GetStatusQuery());

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IReadOnlyCollection<ModuleStatus>>> GetStatus([FromRoute] int id)
        {
            var result = await _mediator.Send(new GetModuleStatusQuery
            {
                Id = id
            });

            return Ok(result);
        }
    }
}