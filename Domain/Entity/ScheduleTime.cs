﻿using System;

namespace Domain.Entity
{
    public class ScheduleTime
    {
        public DayOfWeek Day { get; set; }
        public byte Hour { get; set; }
        public byte Minutes { get; set; }
    }
}