using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartApartmentSystem.Application.Devices.WaterTemperature;
using SmartApartmentSystem.Domain.Entity;
using SmartApartmentSystem.Domain.WaterTemperature;

namespace SmartApartmentSystem.Application.Devices.WaterTemperature.Queries
{
    public class GetTempQuery : IRequest<ModuleStatus>
    {
        public WaterTempChannels Type { get; set; }
    }

    public class GetTempQueryHandler : IRequestHandler<GetTempQuery, ModuleStatus>
    {
        private readonly IWaterTemperatureDevice _temperature;

        public GetTempQueryHandler(IWaterTemperatureDevice temperature)
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
