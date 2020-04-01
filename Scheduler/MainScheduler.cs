using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SmartApartmentSystem.Domain.Entity;
using SmartApartmentSystem.Domain.Entity.Enums;
using SmartApartmentSystem.RaspberryIO.Temperature;

namespace SmartApartmentSystem.Scheduler
{
    public class MainScheduler : IDisposable
    {
        private readonly Timer _timer;
        private readonly TemperatureDevice _device;
        private readonly Queue<StatusWithTime> _queue = new Queue<StatusWithTime>();
        public MainScheduler(TemperatureDevice device)
        {
            _device = device;
            var allTimes = new[]
            {
                new StatusWithTime
                {
                    Type = ModuleTypeEnum.Floor,
                    Hour = 7,
                    Minutes = 30,
                    Status = 1
                },
                new StatusWithTime
                {
                    Type = ModuleTypeEnum.Floor,
                    Hour = 10,
                    Minutes = 0,
                    Status = 0
                },
                new StatusWithTime
                {
                    Type = ModuleTypeEnum.Floor,
                    Hour = 18,
                    Minutes = 0,
                    Status = 1
                },
                new StatusWithTime
                {
                    Type = ModuleTypeEnum.Floor,
                    Hour = 19,
                    Minutes = 30,
                    Status = 0
                },
                new StatusWithTime
                {
                    Type = ModuleTypeEnum.Floor,
                    Hour = 21,
                    Minutes = 0,
                    Status = 1
                },
                new StatusWithTime
                {
                    Type = ModuleTypeEnum.Floor,
                    Hour = 22,
                    Minutes = 0,
                    Status = 0
                }
            };
            SetQueue(allTimes);
            //device.ChannelStatusesChanged += DeviceOnChannelStatusesChanged;
            _timer = new Timer(CheckScheduledTimes, this, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10));
        }

        private void CheckScheduledTimes(object state)
        {
            var nextTime = _queue.Peek();
            var now = DateTime.Now;
            if (nextTime.Hour == now.Hour && nextTime.Minutes == now.Minute)
            {
                _device.SetRegister(nextTime.Type == ModuleTypeEnum.Boiler ? TempChannels.Boiler : TempChannels.Floor,
                    nextTime.Status);
                _queue.Dequeue();
                _queue.Enqueue(nextTime);
            }
        }

        private void SetQueue(StatusWithTime[] times)
        {
            var now = DateTime.Now.TimeOfDay;
            var filteredTimes = times.OrderBy(x => x.Day).ThenBy(x => x.Hour).ThenBy(x => x.Minutes)
                .Select((time, index) => new {index, time}).ToArray();
            var nextRun = filteredTimes.FirstOrDefault(x => x.time.Time > now)
                              ?.index ?? 0;
            foreach (var filteredTime in filteredTimes.Where(f => f.index >= nextRun))
            {
                _queue.Enqueue(filteredTime.time);
            }
            foreach (var filteredTime in filteredTimes.Where(f => f.index < nextRun))
            {
                _queue.Enqueue(filteredTime.time);
            }
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
