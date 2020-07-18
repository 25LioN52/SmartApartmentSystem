using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartApartmentSystem.Domain.Entity;

namespace SmartApartmentSystem.Data.Extensions
{
    public static class ScheduleExtensions
    {
        //public static Schedule GetSchedule(this ICollection<Schedule> schedules, DayOfWeek day, byte hour, byte minutes)
        //{
        //    return schedules.FirstOrDefault(s =>
        //        s.Day == (int)day && s.Hour == hour &&
        //        s.Minutes == minutes);
        //}

        //public static async Task<Schedule> GetSchedule(this DbSet<Schedule> schedules, int moduleId, DayOfWeek day, byte hour, byte minutes)
        //{
        //    return await schedules.FirstOrDefaultAsync(s =>
        //        s.ModuleId == moduleId && s.Day == (int)day && s.Hour == hour && s.Minutes == minutes);
        //}
    }
}
