using Domain.Entity;
using Domain.Entity.Enums;
using MediatR;

namespace Commands
{
    public class SetStatusCommand : IRequest<ResultStatus>
    {
        public ModuleTypeEnum Type { get; set; }
        public byte Status { get; set; }
        public ScheduleTime Schedule { get; set; }
    }
}
