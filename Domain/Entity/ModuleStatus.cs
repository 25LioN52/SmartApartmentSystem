using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entity
{
    public class ModuleStatus
    {
        public byte Id { get; set; }
        public string Name { get; set; }
        public byte ActualStatus { get; set; }
        public byte ExpectedStatus { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsActive { get; set; }
    }
}
