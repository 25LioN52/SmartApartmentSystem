using MediatR;
using SmartApartmentSystem.Domain.Entity;

namespace SmartApartmentSystem.Queries
{
    public class GetModuleStatusQuery : IRequest<ModuleStatus>
    {
        public int Id { get; set; }
    }
}
