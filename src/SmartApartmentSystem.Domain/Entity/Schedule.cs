using System;

namespace SmartApartmentSystem.Domain.Entity
{
    public class Schedule : ScheduleTime
    {
        public int ScheduleId { get; set; }
        public byte ModuleId { get; set; }
        public byte Status { get; set; }
        public TimeSpan Time => new TimeSpan(Hour, Minutes, 0);

        public virtual Module Module { get; set; }
    }
}
