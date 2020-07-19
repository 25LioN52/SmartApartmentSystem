using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SmartApartmentSystem.Application.History.Queries
{
    public class GetFloorHistoryQuery : IRequest<int[]>
    {
        public DateTime Day { get; set; }
    }

    public class GetFloorHistoryQueryHandler : IRequestHandler<GetFloorHistoryQuery, int[]>
    {
        public readonly ISasDb _sasDb;
        public GetFloorHistoryQueryHandler(ISasDb sasDb)
        {
            _sasDb = sasDb;
        }
        public async Task<int[]> Handle(GetFloorHistoryQuery request, CancellationToken cancellationToken)
        {
            var first = await _sasDb.ModuleActuals
                .Where(m => m.ModuleId == 1)
                .OrderByDescending(d => d.ChangeDate).FirstOrDefaultAsync(m => m.ChangeDate < request.Day.Date);
            var todayEvents = await _sasDb.ModuleActuals.Where(m => m.ModuleId == 1 && m.ChangeDate >= request.Day.Date).ToArrayAsync();
            var grouped = todayEvents.GroupBy(e => e.ChangeDate.Hour).Select(g => new { hour = g.Key, turnedOn = g.Any(l => l.IsActive) });
            var result = new int[DateTime.Now.Hour + 1];
            var temp = (first?.IsActive ?? false) ? 1 : 0;
            for (var i = 0; i < result.Length; i++)
            {
                result[i] = (grouped.FirstOrDefault(g => g.hour == i)?.turnedOn ?? false) ? 1 : 0;
                temp = result[i];
            }

            return result;
        }
    }
}
