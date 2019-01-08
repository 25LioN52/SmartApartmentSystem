using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
    public class Module
    {
        public byte Id { get; set; }
        public string Name { get; set; }
        public byte ActualStatus { get; set; }
        public byte ExpectedStatus { get; set; }
        public int DeviceId { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}
