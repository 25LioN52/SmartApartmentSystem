using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartApartmentSystem.Domain.Entity;
using SmartApartmentSystem.Domain.Entity.Enums;
using SmartApartmentSystem.RaspberryIO.Temperature;
using SmartApartmentSystem.Services;

namespace SmartApartmentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FloorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FloorController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<ActionResult<ModuleStatus>> GetTemperature()
        {
            var result = await _mediator.Send(new GetTempQuery
            {
                Type = TempChannels.Floor
            }, CancellationToken.None);

            return result;
        }

        [HttpPost("{value}")]
        public async Task<IActionResult> UpdateTemperature([FromRoute] byte value, [FromBody] ScheduleTime schedule)
        {
            var result = await _mediator.Send(new SetStatusCommand
            {
                Status = value,
                Schedule = schedule,
                Type = ModuleTypeEnum.Floor
            }, CancellationToken.None);

            return StatusCode((int)result);
        }
    }
}