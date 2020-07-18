using SmartApartmentSystem.Application.Devices.WaterTemperature;
using System;
using SmartApartmentSystem.Domain.WaterTemperature;
using SmartApartmentSystem.Domain.Entity;
using SmartApartmentSystem.Domain.Extensions;

namespace SmartApartmentSystem.Application.Jobs
{
    public class Listener
    {
        private readonly ISasDb _sasDb;

        public Listener(ISasDb sasDb, IWaterTemperatureDevice device)
        {
            _sasDb = sasDb;
            device.ChannelStatusesChanged += Temperature_ChannelStatusesChanged;
        }

        private void Temperature_ChannelStatusesChanged(object sender, ChannelStatusesChangedEventArgs<WaterTempChannels> e)
        {
            foreach(var status in e.ChannelStatuses)
            {
                _sasDb.ModuleActuals.Add(new ModuleActual
                {
                    ActualStatus = status.Value.ActualStatus,
                    ChangeDate = DateTime.Now,
                    IsActive = status.Value.IsActive,
                    ModuleId = status.Key.ToGlobalType()
                });
            }
            _sasDb.SaveChanges();
        }
    }
}
