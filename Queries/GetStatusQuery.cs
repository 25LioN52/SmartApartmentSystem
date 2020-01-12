using System.Collections.Generic;
using MediatR;
using SmartApartmentSystem.Domain.Entity;

namespace SmartApartmentSystem.Queries
{
    public class GetStatusQuery : IRequest<IReadOnlyCollection<ModuleStatus>>
    {

    }
}
