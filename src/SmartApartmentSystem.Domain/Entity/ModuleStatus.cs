﻿using System.Collections.Generic;

namespace SmartApartmentSystem.Domain.Entity
{
    public class ModuleStatus
    {
        public byte? ActualStatus { get; set; }
        public byte? ExpectedStatus { get; set; }
        public bool? IsDisabled { get; set; }
        public bool IsActive { get; set; }

        public override bool Equals(object obj)
        {
            if ((obj == null) || GetType() != obj.GetType())
            {
                return false;
            }

            var p = (ModuleStatus)obj;
            return this == p;
        }

        public static bool operator ==(ModuleStatus x, ModuleStatus y) 
            => x?.ActualStatus == y?.ActualStatus
                    && x?.ExpectedStatus == y?.ExpectedStatus
                    && x?.IsActive == y?.IsActive
                    && x?.IsDisabled == y?.IsDisabled;

        public static bool operator !=(ModuleStatus x, ModuleStatus y)
            => !(x == y);

        public override int GetHashCode()
        {
            var hashCode = 1663982080;
            hashCode = hashCode * -1521134295 + EqualityComparer<byte?>.Default.GetHashCode(ActualStatus ?? 0);
            hashCode = hashCode * -1521134295 + EqualityComparer<byte?>.Default.GetHashCode(ExpectedStatus ?? 0);
            hashCode = hashCode * -1521134295 + EqualityComparer<bool?>.Default.GetHashCode(IsDisabled ?? false);
            hashCode = hashCode * -1521134295 + IsActive.GetHashCode();
            return hashCode;
        }
    }
}
