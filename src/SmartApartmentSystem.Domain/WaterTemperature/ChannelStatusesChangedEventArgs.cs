using System;
using System.Collections.Generic;
using SmartApartmentSystem.Domain.Entity;

namespace SmartApartmentSystem.Domain.WaterTemperature
{
    public class ChannelStatusesChangedEventArgs<T> : EventArgs
    {
        /// <summary>
        /// The channel statuses.
        /// </summary>
        public IReadOnlyDictionary<T, ModuleStatus> ChannelStatuses { get; private set; }

        /// <summary>
        /// Initialize event arguments.
        /// </summary>
        /// <param name="channelStatuses">The channel statuses.</param>
        public ChannelStatusesChangedEventArgs(IReadOnlyDictionary<T, ModuleStatus> channelStatuses) : base()
        {
            ChannelStatuses = channelStatuses;
        }
    }
}
