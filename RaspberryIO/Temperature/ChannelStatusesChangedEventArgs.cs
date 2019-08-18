using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryIO.Temperature
{
    public class ChannelStatusesChangedEventArgs : EventArgs
    {
        /// <summary>
        /// The channel statuses.
        /// </summary>
        public IReadOnlyDictionary<TempChannels, (bool status, byte temperature)> ChannelStatuses { get; private set; }

        /// <summary>
        /// Initialize event arguments.
        /// </summary>
        /// <param name="channelStatuses">The channel statuses.</param>
        public ChannelStatusesChangedEventArgs(IReadOnlyDictionary<TempChannels, (bool status, byte temperature)> channelStatuses) : base()
        {
            ChannelStatuses = channelStatuses;
        }
    }
}
