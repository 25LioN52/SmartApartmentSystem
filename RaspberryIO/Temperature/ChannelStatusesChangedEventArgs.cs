using System;
using System.Collections.Generic;
using SmartApartmentSystem.Domain.Entity;

namespace SmartApartmentSystem.RaspberryIO.Temperature
{
    public class ChannelStatusesChangedEventArgs : EventArgs
    {
        /// <summary>
        /// The channel statuses.
        /// </summary>
        public IReadOnlyDictionary<TempChannels, ModuleStatus> ChannelStatuses { get; private set; }

        /// <summary>
        /// Initialize event arguments.
        /// </summary>
        /// <param name="channelStatuses">The channel statuses.</param>
        public ChannelStatusesChangedEventArgs(IReadOnlyDictionary<TempChannels, ModuleStatus> channelStatuses) : base()
        {
            ChannelStatuses = channelStatuses;
        }
    }
}
