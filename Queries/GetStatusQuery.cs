using System.Collections.Generic;
using Domain.Entity;
using MediatR;

namespace Queries
{
    public class GetStatusQuery : IRequest<IReadOnlyCollection<ModuleStatus>>
    {

    }
}
