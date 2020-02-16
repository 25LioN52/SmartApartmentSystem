using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Device.I2c;
using System.Threading;
using SmartApartmentSystem.Domain.Entity;

namespace SmartApartmentSystem.RaspberryIO.Temperature
{
    public class TemperatureDevice : IDisposable
    {
        /// <summary>
        /// MPR121 Default I2C Address.
        /// </summary>
        public static readonly byte DefaultI2cAddress = 0x5A;

        private static readonly int CHANNELS_NUMBER = Enum.GetValues(typeof(TempChannels)).Length;

        private I2cDevice _device;
        private Timer _timer;

        private readonly Dictionary<TempChannels, ModuleStatus> _statuses;

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
            get => _periodRefresh;

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
        /// Initialize a Temperature controller.
        /// </summary>
        /// <param name="device">The i2c device.</param>
        /// <param name="periodRefresh">The period in milliseconds of refresing the channel statuses.</param>
        public TemperatureDevice(int periodRefresh = 1000)
        {
            var settings = new I2cConnectionSettings(1, 0x10);
            _device = I2cDevice.Create(settings);
            _timer = new Timer(RefreshChannelStatuses, this, Timeout.Infinite, Timeout.Infinite);

            _statuses = new Dictionary<TempChannels, ModuleStatus>();
            foreach (TempChannels channel in Enum.GetValues(typeof(TempChannels)))
            {
                _statuses.Add(channel, null);
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
        public IReadOnlyDictionary<TempChannels, ModuleStatus> ReadChannelStatuses()
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
        public ModuleStatus ReadChannelStatus(TempChannels channel)
        {
            RefreshChannelStatuses();

            return _statuses[channel];
        }

        public void SetRegister(TempChannels channel, byte value)
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

            Span<byte> buffer = stackalloc byte[CHANNELS_NUMBER * 2];
            _device.Read(buffer);

            bool isStatusChanged = false;
            for (var i = 0; i < CHANNELS_NUMBER; i++)
            {
                short rawStatus = BinaryPrimitives.ReadInt16LittleEndian(buffer.Slice(i * 2, 2));

                var newStatus = new ModuleStatus
                {
                    IsDisabled = (rawStatus & 1) > 0,
                    IsActive = ((rawStatus >> 1) & 1) > 0,
                    ActualStatus = (byte)((rawStatus >> 2) & 0x3F),
                    ExpectedStatus = (byte)((rawStatus >> 8) & 0x3F)
                };
                if (_statuses[(TempChannels)i] != newStatus)
                {
                    _statuses[(TempChannels)i] = newStatus;
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
