using SmartApartmentSystem.Application.Devices.WaterTemperature;
using SmartApartmentSystem.Domain.WaterTemperature;
using MediatR;
using System;
using SmartApartmentSystem.Application.History.Command;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Collections.Generic;
using SmartApartmentSystem.Domain.Entity;

namespace SmartApartmentSystem.API.Infrastructure
{
    public class Listener
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<Listener> _logger;

        public Listener(IWaterTemperatureDevice device, IServiceProvider serviceProvider, ILogger<Listener> logger)
        {
            _serviceProvider = serviceProvider;
            device.ChannelStatusesChanged += Temperature_ChannelStatusesChanged;
            _logger = logger;
        }

        private void Temperature_ChannelStatusesChanged(object sender, ChannelStatusesChangedEventArgs<WaterTempChannels> e)
        {
            _logger.LogInformation("Got StatusChangedEvent");
            try
            {
                UpdateAndNitifyAsync(e.ChannelStatuses).Wait();
            }
            catch (Exception exception)
            {
                _logger.LogError("Exception");
                _logger.LogError("There was an error {0}", exception);
            }
        }

        private async Task UpdateAndNitifyAsync(IReadOnlyDictionary<WaterTempChannels, ModuleStatus> newChannels)
        {
            var model = new UpdateActualStatusCommand
            {
                Model = newChannels
            };
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Send(model);
            var hub = scope.ServiceProvider.GetRequiredService<IHubContext<SignalRHub>>();
            await hub.Clients.All.SendAsync("statusesChanged", true);
        }
    }
}
