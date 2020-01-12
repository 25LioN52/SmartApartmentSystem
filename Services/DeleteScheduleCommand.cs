using MediatR;
using SmartApartmentSystem.Domain.Entity;
using SmartApartmentSystem.Domain.Entity.Enums;

namespace SmartApartmentSystem.Services
{
    public class DeleteScheduleCommand : IRequest<ResultStatus>
    {
        public ModuleTypeEnum Type { get; set; }
        public ScheduleTime Schedule { get; set; }
    }
}
