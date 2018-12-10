using System;
using Domain.Entity.Enums;

namespace Domain.Modules
{
    public abstract class ModuleBase : IModule
    {
        protected ModuleTypeEnum TypeEnum;

        public bool WasPingedSuccessfully()
        {
            throw new NotImplementedException();
        }
    }
}
