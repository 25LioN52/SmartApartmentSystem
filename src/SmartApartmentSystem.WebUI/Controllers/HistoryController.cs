using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartApartmentSystem.Application.History.Queries;

namespace SmartApartmentSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private readonly IMediator _mediatr;

        public HistoryController(IMediator mediator)
        {
            _mediatr = mediator;
        }

        [HttpGet("Temperature")]
        public async Task<IActionResult> GetTemperatureHistory()
        {
            var result = await _mediatr.Send(new GetTemperatureHistoryQuery
            {
                Day = DateTime.Today
            });

            return Ok(result);
        }

        [HttpGet("Floor")]
        public async Task<IActionResult> GetFloorHistory()
        {
            var result = await _mediatr.Send(new GetFloorHistoryQuery
            {
                Day = DateTime.Today
            });

            return Ok(result);
        }
    }
}
