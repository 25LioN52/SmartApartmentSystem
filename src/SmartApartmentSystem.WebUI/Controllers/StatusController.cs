using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartApartmentSystem.Domain.Entity;
using System.Linq;
using SmartApartmentSystem.Infrastructure.Data;

namespace SmartApartmentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly SasDbContext _context;

        public StatusController(IMediator mediator, SasDbContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        [HttpGet]
        public ActionResult<string> GetStatuses()
        {
            var tr = _context.Modules.ToArray();
            var result = $"{DateTime.Now.Hour} {DateTime.Now.Minute} {tr.FirstOrDefault()}";

            return Ok(result);
        }

        //[HttpGet("{id}")]
        //public async Task<ActionResult<IReadOnlyCollection<ModuleStatus>>> GetStatus([FromRoute] int id)
        //{
        //    var result = await _mediator.Send(new GetModuleStatusQuery
        //    {
        //        Id = id
        //    });

        //    return Ok(result);
        //}
    }
}