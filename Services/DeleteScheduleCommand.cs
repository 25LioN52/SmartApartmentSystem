using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entity;
using Domain.Entity.Enums;
using MediatR;

namespace Commands
{
    public class DeleteScheduleCommand : IRequest<ResultStatus>
    {
        public ModuleTypeEnum Type { get; set; }
        public Schedule Schedule { get; set; }
    }
}
