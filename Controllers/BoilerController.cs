using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SmartApartmentSystem.Domain.Entity;
using SmartApartmentSystem.Domain.Entity.Enums;
using SmartApartmentSystem.RaspberryIO.Temperature;
using SmartApartmentSystem.Services;

namespace SmartApartmentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoilerController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<BoilerController> _logger;

        public BoilerController(IMediator mediator, ILogger<BoilerController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ModuleStatus>> GetTemperature()
        {
            _logger.LogInformation("tratata");
            var result = await _mediator.Send(new GetTempQuery
            {
                Type = TempChannels.Boiler
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
