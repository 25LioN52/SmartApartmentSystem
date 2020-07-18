using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartApartmentSystem.Application.Devices.WaterTemperature.Commands;
using SmartApartmentSystem.Application.Devices.WaterTemperature.Queries;
using SmartApartmentSystem.Domain.Entity;
using SmartApartmentSystem.Domain.WaterTemperature;

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
                Type = WaterTempChannels.Floor
            }, CancellationToken.None);

            return result;
        }

        [HttpPost("{value}")]
        public async Task<IActionResult> UpdateTemperature([FromRoute] bool value, [FromBody] ScheduleTime schedule)
        {
            var result = await _mediator.Send(new SetStatusCommand
            {
                Status = (byte) (value ? 1 : 0),
                Schedule = schedule,
                Type = WaterTempChannels.Floor
            }, CancellationToken.None);

            return StatusCode((int)result);
        }
    }
}