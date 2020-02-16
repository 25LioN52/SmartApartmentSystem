using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartApartmentSystem.Data;
using SmartApartmentSystem.Data.Extensions;
using SmartApartmentSystem.Data.Models;
using SmartApartmentSystem.Domain.Entity;
using SmartApartmentSystem.Domain.Entity.Enums;
using SmartApartmentSystem.RaspberryIO.Temperature;

namespace SmartApartmentSystem.Services
{
    public class SetStatusCommand : IRequest<ResultStatus>
    {
        public ModuleTypeEnum Type { get; set; }
        public byte Status { get; set; }
        public ScheduleTime Schedule { get; set; }
    }

    public class SetStatusCommandHandler : IRequestHandler<SetStatusCommand, ResultStatus>
    {
        private readonly SasDbContext _context;
        private readonly TemperatureDevice _temperature;

        public SetStatusCommandHandler(SasDbContext context, TemperatureDevice temperature)
        {
            _context = context;
            _temperature = temperature;
        }

        public Task<ResultStatus> Handle(SetStatusCommand request, CancellationToken cancellationToken)
        {
            _temperature.SetRegister(TempChannels.Boiler, request.Status);
            /*
            var module = await _context.Modules.Include(m => m.Schedules)
                .FirstOrDefaultAsync(m => m.Id == (int)ModuleTypeEnum.Boiler, cancellationToken: cancellationToken);

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
                        Day = (int)request.Schedule.Day,
                        Hour = request.Schedule.Hour,
                        Minutes = request.Schedule.Minutes,
                        Status = request.Status,
                        ModuleId = module.Id
                    });
                }
            }
            else
            {
                //var myDevice = Pi.I2C.AddDevice(module.DeviceId);

                //myDevice.Write(new[] { module.Id, request.Status });

                //module.ExpectedStatus = request.Status;
            }

            await _context.SaveChangesAsync(cancellationToken);
            */
            return Task.FromResult(ResultStatus.Success);
        }
    }
}
