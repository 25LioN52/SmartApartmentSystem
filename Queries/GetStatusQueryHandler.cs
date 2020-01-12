using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartApartmentSystem.Data;
using SmartApartmentSystem.Domain.Entity;

namespace SmartApartmentSystem.Queries
{
    public class GetStatusQueryHandler : IRequestHandler<GetStatusQuery, IReadOnlyCollection<ModuleStatus>>
    {
        private readonly SasDbContext _context;

        public GetStatusQueryHandler(SasDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyCollection<ModuleStatus>> Handle(GetStatusQuery request, CancellationToken cancellationToken)
        {
            var modules = await _context.Modules.Select(m => new ModuleStatus
            {
                IsActive = m.IsActive,
                ActualStatus = m.ActualStatus,
                ExpectedStatus = m.ExpectedStatus,
                IsDisabled = m.IsDisabled
            }).ToArrayAsync(cancellationToken: cancellationToken);

            return modules;
        }
    }
}
