using Domain.Entity;
using MediatR;

namespace Queries
{
    public class GetSchedulesQuery: IRequest<ScheduleTime>
    {
    }
}
