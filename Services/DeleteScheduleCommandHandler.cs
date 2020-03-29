using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartApartmentSystem.Data;
using SmartApartmentSystem.Data.Extensions;

namespace SmartApartmentSystem.Services
{
    public class DeleteScheduleCommandHandler : IRequestHandler<DeleteScheduleCommand, ResultStatus>
    {
        private readonly SasDbContext _context;

        public DeleteScheduleCommandHandler(SasDbContext context)
        {
            _context = context;
        }

        public async Task<ResultStatus> Handle(DeleteScheduleCommand request, CancellationToken cancellationToken)
        {
            //var schedule = await _context.Schedules.GetSchedule((int) request.Type, request.Schedule.Day,
            //    request.Schedule.Hour, request.Schedule.Minutes);

            //if (schedule == null)
            //{
            //    return ResultStatus.NotFound;
            //}

            //_context.Schedules.Remove(schedule);
            //await _context.SaveChangesAsync(cancellationToken);

            return ResultStatus.Success;
        }
    }
}
