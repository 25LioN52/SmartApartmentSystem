using Domain.Entity;
using System;
using System.Collections.Generic;

namespace RaspberryIO.Temperature
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
