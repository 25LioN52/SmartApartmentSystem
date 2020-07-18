using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartApartmentSystem.Application;
using SmartApartmentSystem.Domain.Entity;
using SmartApartmentSystem.Infrastructure.Data;
using System.Threading.Tasks;

namespace SmartApartmentSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DbController : ControllerBase
    {
        private readonly ISasDb _context;

        public DbController(ISasDb context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task AddModule()
        {
            var con = _context as SasDbContext;
            con.Database.Migrate();
            //_context.Modules.Add(module);
            //await _context.SaveChangesAsync();
        }

        [HttpGet]
        public async Task<IActionResult> GetHistory()
        {
            var result= await _context.ModuleActuals.ToArrayAsync();
            return Ok(result);
        }
    }
}
