using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartApartmentSystem.Domain.Entity;
using SmartApartmentSystem.Queries;

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
        public ActionResult<string> GetStatuses()
        {
            var result = $"{DateTime.Now.Hour} {DateTime.Now.Minute}";

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