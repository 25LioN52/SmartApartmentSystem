using System;

namespace SmartApartmentSystem.Domain.Entity
{
    public class StatusWithTime : ScheduleTime
    {
        public int Type { get; set; }
        public byte Status { get; set; }
        public TimeSpan Time => new TimeSpan(Hour, Minutes, 0);
    }
}
