using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SmartApartmentSystem.Domain.Entity;
using SmartApartmentSystem.Domain.WaterTemperature;
using SmartApartmentSystem.Application.Devices.WaterTemperature.Commands;
using SmartApartmentSystem.Application.Devices.WaterTemperature.Queries;

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
            var result = await _mediator.Send(new GetTempQuery
            {
                Type = WaterTempChannels.Boiler
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
                Type = WaterTempChannels.Boiler
            }, CancellationToken.None);

            return StatusCode((int)result);
        }

        //[HttpDelete("{day}/{hour}/{minutes}")]
        //public async Task<IActionResult> DeleteTemperature([FromRoute] DayOfWeek day, [FromRoute] byte hour,
        //    [FromRoute] byte minutes)
        //{
        //    var result = await _mediator.Send(new DeleteScheduleCommand
        //    {
        //        Schedule = new ScheduleTime
        //        {
        //            Day = day,
        //            Hour = hour,
        //            Minutes = minutes
        //        },
        //        Type = ModuleTypeEnum.Boiler
        //    }, CancellationToken.None);

        //    return StatusCode((int)result);
        //}
    }
}
