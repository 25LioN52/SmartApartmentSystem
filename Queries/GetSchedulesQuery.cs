using MediatR;
using SmartApartmentSystem.Domain.Entity;

namespace SmartApartmentSystem.Queries
{
    public class GetSchedulesQuery: IRequest<ScheduleTime>
    {
    }
}
