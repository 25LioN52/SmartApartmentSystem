using Microsoft.EntityFrameworkCore;
using SmartApartmentSystem.Domain.Entity;
using System.Threading;
using System.Threading.Tasks;

namespace SmartApartmentSystem.Application
{
    public interface ISasDb
    {
        DbSet<Module> Modules { get; set; }
        DbSet<ModuleActual> ModuleActuals { get; set; }
        DbSet<Schedule> Schedules { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        int SaveChanges();
    }
}
