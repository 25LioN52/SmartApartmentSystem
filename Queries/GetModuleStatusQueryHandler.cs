using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartApartmentSystem.Data;
using SmartApartmentSystem.Domain.Entity;

namespace SmartApartmentSystem.Queries
{
    public class GetModuleStatusQueryHandler : IRequestHandler<GetModuleStatusQuery, ModuleStatus>
    {
        private readonly SasDbContext _context;

        public GetModuleStatusQueryHandler(SasDbContext context)
        {
            _context = context;
        }

        public async Task<ModuleStatus> Handle(GetModuleStatusQuery request, CancellationToken cancellationToken)
        {
            var module = await _context.Modules.Where(m => m.Id == request.Id).Select(m => new ModuleStatus
            {
                IsActive = m.IsActive,
                ActualStatus = m.ActualStatus,
                ExpectedStatus = m.ExpectedStatus,
                IsDisabled = m.IsDisabled
            }).FirstOrDefaultAsync(cancellationToken: cancellationToken);

            return module;
        }
    }
}
