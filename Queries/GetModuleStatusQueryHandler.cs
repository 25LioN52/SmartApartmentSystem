using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Data;
using Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Queries
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
                Id = m.Id,
                IsActive = m.IsActive,
                ActualStatus = m.ActualStatus,
                Name = m.Name,
                ExpectedStatus = m.ExpectedStatus,
                IsDisabled = m.IsDisabled
            }).FirstOrDefaultAsync(cancellationToken: cancellationToken);

            return module;
        }
    }
}
