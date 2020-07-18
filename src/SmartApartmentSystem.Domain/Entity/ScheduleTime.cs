using System;

namespace SmartApartmentSystem.Domain.Entity
{
    public class ScheduleTime
    {
        public bool Sunday { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public byte Hour { get; set; }
        public byte Minutes { get; set; }
        public DateTime? Date { get; set; }
    }
}
