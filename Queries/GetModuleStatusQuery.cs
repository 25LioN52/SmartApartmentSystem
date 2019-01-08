using Domain.Entity;
using MediatR;

namespace Queries
{
    public class GetModuleStatusQuery : IRequest<ModuleStatus>
    {
        public int Id { get; set; }
    }
}
