using System;
using System.Collections.Generic;

namespace SmartApartmentSystem.RaspberryIO.Light
{
    public class ChannelStatusesChangedEventArgs : EventArgs
    {
        /// <summary>
        /// The channel statuses.
        /// </summary>
        public IReadOnlyDictionary<LightChannels, bool> ChannelStatuses { get; private set; }

        /// <summary>
        /// Initialize event arguments.
        /// </summary>
        /// <param name="channelStatuses">The channel statuses.</param>
        public ChannelStatusesChangedEventArgs(IReadOnlyDictionary<LightChannels, bool> channelStatuses) : base()
        {
            ChannelStatuses = channelStatuses;
        }
    }
}
