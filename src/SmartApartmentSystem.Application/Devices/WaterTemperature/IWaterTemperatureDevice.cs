using SmartApartmentSystem.Domain.Entity;
using SmartApartmentSystem.Domain.WaterTemperature;
using System;
using System.Collections.Generic;

namespace SmartApartmentSystem.Application.Devices.WaterTemperature
{
    public interface IWaterTemperatureDevice
    {
        IReadOnlyDictionary<WaterTempChannels, ModuleStatus> ReadChannelStatuses();
        ModuleStatus ReadChannelStatus(WaterTempChannels channel);
        void SetRegister(WaterTempChannels channel, byte value);
        event EventHandler<ChannelStatusesChangedEventArgs<WaterTempChannels>> ChannelStatusesChanged;
    }
}
