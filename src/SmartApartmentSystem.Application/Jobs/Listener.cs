using SmartApartmentSystem.Application.Devices.WaterTemperature;
using SmartApartmentSystem.Domain.WaterTemperature;
using MediatR;
using System;
using SmartApartmentSystem.Application.History.Command;
using Microsoft.Extensions.DependencyInjection;

namespace SmartApartmentSystem.Application.Jobs
{
    public class Listener
    {
        private readonly IServiceProvider _serviceProvider;

        public Listener(IWaterTemperatureDevice device, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            device.ChannelStatusesChanged += Temperature_ChannelStatusesChanged;
        }

        private void Temperature_ChannelStatusesChanged(object sender, ChannelStatusesChangedEventArgs<WaterTempChannels> e)
        {
            var model = new UpdateActualStatusCommand
            {
                Model = e.ChannelStatuses
            };
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            _ = mediator.Send(model).Result;
        }
    }
}
