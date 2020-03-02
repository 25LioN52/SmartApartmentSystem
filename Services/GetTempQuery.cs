using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartApartmentSystem.Data;
using SmartApartmentSystem.Data.Extensions;
using SmartApartmentSystem.Domain.Entity;
using SmartApartmentSystem.Domain.Entity.Enums;
using SmartApartmentSystem.RaspberryIO.Temperature;

namespace SmartApartmentSystem.Services
{
    public class GetTempQuery : IRequest<ModuleStatus>
    {
        public TempChannels Type { get; set; }
    }

    public class GetTempQueryHandler : IRequestHandler<GetTempQuery, ModuleStatus>
    {
        private readonly TemperatureDevice _temperature;

        public GetTempQueryHandler(TemperatureDevice temperature)
        {
            _temperature = temperature;
        }

        public Task<ModuleStatus> Handle(GetTempQuery request, CancellationToken cancellationToken)
        {
            var result = _temperature.ReadChannelStatus(request.Type);

            return Task.FromResult(result);
        }
    }
}
