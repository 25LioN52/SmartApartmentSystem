using System.Collections.Generic;

namespace SmartApartmentSystem.Domain.Entity
{
    public class Module
    {
        public byte Id { get; set; }
        public string Name { get; set; }
        public byte? ExpectedStatus { get; set; }
        public bool? IsDisabled { get; set; }

        public virtual ICollection<Schedule> Schedules { get; set; }
        public virtual ICollection<ModuleActual> StatusHistory { get; set; }
    }
}
