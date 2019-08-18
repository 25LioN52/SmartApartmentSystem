using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Device.I2c;
using System.Threading;

namespace RaspberryIO.Light
{
    public class LightDevice : IDisposable
    {
        /// <summary>
        /// MPR121 Default I2C Address.
        /// </summary>
        public static readonly byte DefaultI2cAddress = 0x5A;

        private static readonly int CHANNELS_NUMBER = Enum.GetValues(typeof(LightChannels)).Length;

        private I2cDevice _device;
        private Timer _timer;

        private readonly Dictionary<LightChannels, bool> _statuses;

        private int _periodRefresh;

        /// <summary>
        /// Notifies about a the channel statuses have been changed.
        /// Refresh period can be changed by setting PeriodRefresh property.
        /// </summary>
        public event EventHandler<ChannelStatusesChangedEventArgs> ChannelStatusesChanged;

        /// <summary>
        /// Gets or sets the period in milliseconds to refresh the channels statuses.
        /// </summary>
        /// <remark>
        /// Set value 0 to stop the automatically refreshing. Setting the value greater than 0 will start/update auto-refresh.
        /// </remark>
        public int PeriodRefresh
        {
            get { return _periodRefresh; }

            set
            {
                _periodRefresh = value;

                if (_periodRefresh > 0)
                {
                    _timer.Change(TimeSpan.FromMilliseconds(_periodRefresh), TimeSpan.FromMilliseconds(_periodRefresh));
                }
                else
                {
                    // Disable the auto-refresh.
                    _timer.Change(Timeout.Infinite, Timeout.Infinite);
                }
            }
        }

        /// <summary>
        /// Initialize a MPR121 controller.
        /// </summary>
        /// <param name="device">The i2c device.</param>
        /// <param name="periodRefresh">The period in milliseconds of refresing the channel statuses.</param>
        public LightDevice(I2cDevice device, int periodRefresh = -1)
        {
            _device = device;
            _timer = new Timer(RefreshChannelStatuses, this, Timeout.Infinite, Timeout.Infinite);

            _statuses = new Dictionary<LightChannels, bool>();
            foreach (LightChannels channel in Enum.GetValues(typeof(LightChannels)))
            {
                _statuses.Add(channel, false);
            }

            PeriodRefresh = periodRefresh;
        }

        public void Dispose()
        {
            if (_device != null)
            {
                _device.Dispose();
                _device = null;
            }

            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
            }
        }

        /// <summary>
        /// Reads the channel statuses of MPR121 controller.
        /// </summary>
        public IReadOnlyDictionary<LightChannels, bool> ReadChannelStatuses()
        {
            RefreshChannelStatuses();

            return _statuses.ToImmutableDictionary();
        }

        /// <summary>
        /// Reads the channel status of MPR121 controller.
        /// </summary>
        /// <param name="channel">The channel to read status.</param>
        /// <remark>
        /// Please use ReadChannelStatuses() if you need to read statuses of multiple channels.
        /// Using this method several times to read status for several channels can affect the performance.
        /// </remark>
        public bool ReadChannelStatus(LightChannels channel)
        {
            RefreshChannelStatuses();

            return _statuses[channel];
        }

        public void SetRegister(LightChannels channel, byte value)
        {
            Span<byte> data = stackalloc byte[] { (byte)channel, value };
            _device.Write(data);
        }

        /// <summary>
        /// The callback function for timer to refresh channels statuses.
        /// </summary>
        private void RefreshChannelStatuses(object state)
        {
            RefreshChannelStatuses();
        }

        /// <summary>
        /// Refresh the channel statuses.
        /// </summary>
        private void RefreshChannelStatuses()
        {
            // Pause the auto-refresh to prevent possible collisions.
            var periodRefresh = PeriodRefresh;
            PeriodRefresh = 0;

            Span<byte> buffer = stackalloc byte[2];
            _device.Read(buffer);

            short rawStatus = BinaryPrimitives.ReadInt16LittleEndian(buffer);
            bool isStatusChanged = false;
            for (var i = 0; i < CHANNELS_NUMBER; i++)
            {
                bool status = ((1 << i) & rawStatus) > 0;
                if (_statuses[(LightChannels)i] != status)
                {
                    _statuses[(LightChannels)i] = status;
                    isStatusChanged = true;
                }
            }

            if (isStatusChanged)
            {
                OnChannelStatusesChanged();
            }

            // Resume the auto-refresh.
            PeriodRefresh = periodRefresh;
        }

        private void OnChannelStatusesChanged()
        {
            ChannelStatusesChanged?.Invoke(this, new ChannelStatusesChangedEventArgs(_statuses.ToImmutableDictionary()));
        }
    }
}
