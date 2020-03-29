using System;
using SmartApartmentSystem.Domain.Entity.Enums;

namespace SmartApartmentSystem.Domain.Entity
{
    public class StatusWithTime : ScheduleTime
    {
        public ModuleTypeEnum Type { get; set; }
        public byte Status { get; set; }
        public TimeSpan Time => new TimeSpan(Hour, Minutes, 0);
    }
}
