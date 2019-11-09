using Commands;
using Domain.Entity;
using Domain.Entity.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SmartApartmentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoilerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BoilerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> GetTemperature()
        {
            return new[] { "value1", "value2" };
        }

        [HttpPost("{value}")]
        public async Task<IActionResult> UpdateTemperature([FromRoute] byte value, [FromBody] ScheduleTime schedule)
        {
            var result = await _mediator.Send(new SetStatusCommand
            {
                Status = value,
                Schedule = schedule,
                Type = ModuleTypeEnum.Boiler
            }, CancellationToken.None);

            return StatusCode((int)result);
        }

        [HttpDelete("{day}/{hour}/{minutes}")]
        public async Task<IActionResult> DeleteTemperature([FromRoute] DayOfWeek day, [FromRoute] byte hour,
            [FromRoute] byte minutes)
        {
            var result = await _mediator.Send(new DeleteScheduleCommand
            {
                Schedule = new ScheduleTime
                {
                    Day = day,
                    Hour = hour,
                    Minutes = minutes
                },
                Type = ModuleTypeEnum.Boiler
            }, CancellationToken.None);

            return StatusCode((int)result);
        }
    }
}
