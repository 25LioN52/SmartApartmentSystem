using MediatR;
using SmartApartmentSystem.Domain.Entity;
using SmartApartmentSystem.Domain.Entity.Enums;

namespace SmartApartmentSystem.Services
{
    public class SetStatusCommand : IRequest<ResultStatus>
    {
        public ModuleTypeEnum Type { get; set; }
        public byte Status { get; set; }
        public ScheduleTime Schedule { get; set; }
    }
}
