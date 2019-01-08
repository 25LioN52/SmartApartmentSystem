using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Data;
using Data.Extensions;
using Data.Models;
using Domain.Entity.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unosquare.RaspberryIO;

namespace Commands
{
    public class SetStatusCommandHandler : IRequestHandler<SetStatusCommand, ResultStatus>
    {
        private readonly SasDbContext _context;

        public SetStatusCommandHandler(SasDbContext context)
        {
            _context = context;
        }

        public async Task<ResultStatus> Handle(SetStatusCommand request, CancellationToken cancellationToken)
        {
            var module = await _context.Modules.Include(m => m.Schedules)
                .FirstOrDefaultAsync(m => m.Id == (int) ModuleTypeEnum.Boiler, cancellationToken: cancellationToken);

            if (request.Schedule == null && module.ExpectedStatus == request.Status)
            {
                return ResultStatus.NotChanged;
            }

            if (request.Schedule != null)
            {
                var oldSchedule = module.Schedules.GetSchedule(request.Schedule.Day, request.Schedule.Hour,
                    request.Schedule.Minutes);
                if (oldSchedule != null)
                {
                    oldSchedule.Status = request.Status;
                }
                else
                {
                    _context.Schedules.Add(new Schedule
                    {
                        Day = (int) request.Schedule.Day,
                        Hour = request.Schedule.Hour,
                        Minutes = request.Schedule.Minutes,
                        Status = request.Status,
                        ModuleId = module.Id
                    });
                }
            }
            else
            {
                var myDevice = Pi.I2C.AddDevice(module.DeviceId);

                await myDevice.WriteAsync(new[] { module.Id, request.Status });

                module.ExpectedStatus = request.Status;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return ResultStatus.Success;
        }
    }
}
