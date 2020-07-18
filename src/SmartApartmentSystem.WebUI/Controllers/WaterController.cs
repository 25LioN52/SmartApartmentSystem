using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartApartmentSystem.Application.Devices.WaterTemperature.Commands;
using SmartApartmentSystem.Application.Devices.WaterTemperature.Queries;
using SmartApartmentSystem.Domain.WaterTemperature;

namespace SmartApartmentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WaterController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<WaterController> _logger;

        public WaterController(IMediator mediator, ILogger<WaterController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<bool>> GetWaterStatus()
        {
            var result = await _mediator.Send(new GetTempQuery
            {
                Type = WaterTempChannels.Water
            }, CancellationToken.None);

            return result.ActualStatus > 0;
        }

        [HttpPost("{value}")]
        public async Task<IActionResult> UpdateWaterGateway([FromRoute] bool value)
        {
            var result = await _mediator.Send(new SetStatusCommand
            {
                Status = (byte) (value ? 1 : 0),
                Type = WaterTempChannels.Water
            }, CancellationToken.None);

            return StatusCode((int)result);
        }
    }
}
