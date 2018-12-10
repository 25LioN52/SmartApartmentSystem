using System;
using MediatR;

namespace Commands
{
    public class SetBoilerTemperatureCommand : IRequest<ResultStatus>
    {
        public int Temperature { get; set; }
        public DateTime? Time { get; set; }
    }
}
