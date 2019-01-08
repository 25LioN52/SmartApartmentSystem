using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Data;
using Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Commands
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
            var schedule = await _context.Schedules.GetSchedule((int) request.Type, request.Schedule.Day,
                request.Schedule.Hour, request.Schedule.Minutes);

            if (schedule == null)
            {
                return ResultStatus.NotFound;
            }

            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync(cancellationToken);

            return ResultStatus.Success;
        }
    }
}
