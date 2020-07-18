using System;

namespace SmartApartmentSystem.Domain.Entity
{
    public class ModuleActual
    {
        public Guid Id { get; set; }
        public byte ModuleId { get; set; }
        public byte? ActualStatus { get; set; }
        public bool IsActive { get; set; }
        public DateTime ChangeDate { get; set; }

        public virtual Module Module { get; set; }
    }
}
