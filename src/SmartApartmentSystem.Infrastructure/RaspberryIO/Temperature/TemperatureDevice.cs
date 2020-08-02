using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Device.I2c;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Logging;
using SmartApartmentSystem.Application.Devices.WaterTemperature;
using SmartApartmentSystem.Domain.Entity;
using SmartApartmentSystem.Domain.WaterTemperature;

namespace SmartApartmentSystem.Infrastructure.RaspberryIO.Temperature
{
    public class TemperatureDevice : IWaterTemperatureDevice, IDisposable
    {
        /// <summary>
        /// Arduino Default I2C Address.
        /// </summary>
        public static readonly byte DefaultI2cAddress = 0x10;

        private static readonly int CHANNELS_NUMBER = Enum.GetValues(typeof(WaterTempChannels)).Length;

        private I2cDevice _device;
        private Timer _timer;

        private readonly Dictionary<WaterTempChannels, ModuleStatus> _statuses;

        private int _periodRefresh;

        private readonly ILogger<TemperatureDevice> _logger;

        /// <summary>
        /// Notifies about a the channel statuses have been changed.
        /// Refresh period can be changed by setting PeriodRefresh property.
        /// </summary>
        public event EventHandler<ChannelStatusesChangedEventArgs<WaterTempChannels>> ChannelStatusesChanged;

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
        public TemperatureDevice(ILogger<TemperatureDevice> logger)
        {
            _logger = logger;
            _logger.LogInformation("Temperature init");
            var settings = new I2cConnectionSettings(1, 0x10);
            _device = I2cDevice.Create(settings);
            _timer = new Timer(RefreshChannelStatuses, this, Timeout.Infinite, Timeout.Infinite);

            _statuses = new Dictionary<WaterTempChannels, ModuleStatus>();
            foreach (WaterTempChannels channel in Enum.GetValues(typeof(WaterTempChannels)))
            {
                _statuses.Add(channel, null);
            }

            PeriodRefresh = 10000;
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
        /// Reads the channel statuses
        /// </summary>
        public IReadOnlyDictionary<WaterTempChannels, ModuleStatus> ReadChannelStatuses()
        {
            RefreshChannelStatuses();

            return _statuses.ToImmutableDictionary();
        }

        /// <summary>
        /// Reads the channel statu
        /// </summary>
        /// <param name="channel">The channel to read status.</param>
        /// <remark>
        /// Please use ReadChannelStatuses() if you need to read statuses of multiple channels.
        /// Using this method several times to read status for several channels can affect the performance.
        /// </remark>
        public ModuleStatus ReadChannelStatus(WaterTempChannels channel)
        {
            RefreshChannelStatuses();

            return _statuses[channel];
        }

        public void SetRegister(WaterTempChannels channel, byte value)
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
            _logger.LogInformation("Refresh status");
            // Pause the auto-refresh to prevent possible collisions.
            var periodRefresh = PeriodRefresh;
            PeriodRefresh = 0;

            Span<byte> buffer = stackalloc byte[CHANNELS_NUMBER * 2];
            try
            {
                _device.Read(buffer);

                var statusChanged = new List<WaterTempChannels>();
                for (var i = 0; i < CHANNELS_NUMBER; i++)
                {
                    ushort rawStatus = BinaryPrimitives.ReadUInt16BigEndian(buffer.Slice(i * 2, 2));

                    var newStatus = new ModuleStatus
                    {
                        IsDisabled = (rawStatus & 1) > 0,
                        IsActive = (rawStatus >> 1 & 1) > 0,
                        ActualStatus = (byte)(rawStatus >> 2 & 0x3F),
                        ExpectedStatus = (byte)(rawStatus >> 8 & 0x3F)
                    };
                    if (_statuses[(WaterTempChannels)i] != newStatus)
                    {
                        _statuses[(WaterTempChannels)i] = newStatus;
                        statusChanged.Add((WaterTempChannels)i);
                    }
                }

                if (statusChanged.Any())
                {
                    _logger.LogInformation("Status Changed");
                    OnChannelStatusesChanged(statusChanged);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            // Resume the auto-refresh.
            PeriodRefresh = periodRefresh;
        }

        private void OnChannelStatusesChanged(List<WaterTempChannels> list)
        {
            ChannelStatusesChanged?.Invoke(this, new ChannelStatusesChangedEventArgs<WaterTempChannels>(_statuses.Where(s => list.Contains(s.Key))
                .ToImmutableDictionary()));
        }
    }
}
