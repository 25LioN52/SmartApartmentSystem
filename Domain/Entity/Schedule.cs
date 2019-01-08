using System;

namespace Domain.Entity
{
    public class Schedule
    {
        public DayOfWeek Day { get; set; }
        public byte Hour { get; set; }
        public byte Minutes { get; set; }
    }
}
