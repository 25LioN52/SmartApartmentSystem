using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SmartApartmentSystem.Application.History.Queries
{
    public class GetTemperatureHistoryQuery : IRequest<double[]>
    {
        public DateTime Day { get; set; }
    }

    public class GetTemperatureHistoryQueryHandler : IRequestHandler<GetTemperatureHistoryQuery, double[]>
    {
        public readonly ISasDb _sasDb;
        public GetTemperatureHistoryQueryHandler(ISasDb sasDb)
        {
            _sasDb = sasDb;
        }
        public async Task<double[]> Handle(GetTemperatureHistoryQuery request, CancellationToken cancellationToken)
        {
            var first = await _sasDb.ModuleActuals
                .Where(m => m.ModuleId == 0)
                .OrderByDescending(d => d.ChangeDate).FirstOrDefaultAsync(m => m.ChangeDate < request.Day.Date);
            var todayEvents = await _sasDb.ModuleActuals.Where(m => m.ModuleId == 0 && m.ChangeDate >= request.Day.Date).ToArrayAsync();
            var grouped = todayEvents.GroupBy(e => e.ChangeDate.Hour).Select(g => new { hour = g.Key, temperature = g.Average(e => e.ActualStatus) });
            var result = new double[DateTime.Now.Hour + 1];
            var temp = (double)(first?.ActualStatus.Value ?? 24);
            for (var i = 0; i < result.Length; i++)
            {
                result[i] = grouped.FirstOrDefault(g => g.hour == i)?.temperature.Value ?? temp;
                temp = result[i];
            }

            return result;
        }
    }
}
