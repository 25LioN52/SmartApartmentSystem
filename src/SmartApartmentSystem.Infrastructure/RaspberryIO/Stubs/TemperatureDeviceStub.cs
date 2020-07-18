using SmartApartmentSystem.Application.Devices.WaterTemperature;
using SmartApartmentSystem.Domain.Entity;
using SmartApartmentSystem.Domain.WaterTemperature;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace SmartApartmentSystem.Infrastructure.RaspberryIO.Temperature
{
    public class TemperatureDeviceStub : IWaterTemperatureDevice, IDisposable
    {
        private readonly Dictionary<WaterTempChannels, ModuleStatus> _statuses = new Dictionary<WaterTempChannels, ModuleStatus>() 
        {
            { WaterTempChannels.Boiler, new ModuleStatus{ ActualStatus = 28, ExpectedStatus = 24 } },
            { WaterTempChannels.Floor, new ModuleStatus{ ActualStatus = 0, ExpectedStatus = 0 } },
            { WaterTempChannels.Water, new ModuleStatus{ ActualStatus = 1, ExpectedStatus = 1 } }
        };

        public event EventHandler<ChannelStatusesChangedEventArgs<WaterTempChannels>> ChannelStatusesChanged;

        public void Dispose()
        {
        }

        public ModuleStatus ReadChannelStatus(WaterTempChannels channel)
        {
            return _statuses[channel];
        }

        public IReadOnlyDictionary<WaterTempChannels, ModuleStatus> ReadChannelStatuses()
        {
            return _statuses;
        }

        public void SetRegister(WaterTempChannels channel, byte value)
        {
            _statuses[channel].ExpectedStatus = value;
            ChannelStatusesChanged.Invoke(this, new ChannelStatusesChangedEventArgs<WaterTempChannels>(_statuses.Where(s => s.Key == channel)
                .ToImmutableDictionary()));
        }
    }
}
