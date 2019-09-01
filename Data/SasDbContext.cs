using System;
using System.Collections.Generic;
using System.Text;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public sealed class SasDbContext : DbContext
    {
        public DbSet<Module> Modules { get; set; }
        public DbSet<Schedule> Schedules { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=SasDb.db");
        }

        public SasDbContext(DbContextOptions options)
        : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Module>()
                .HasMany(p => p.Schedules)
                .WithOne(b => b.Module)
                .HasForeignKey(p => p.ModuleId);

            modelBuilder.Entity<Module>().HasData(new Module
            {
                Id = 1,
                Name = "Boiler",
                ActualStatus = 0,
                ExpectedStatus = 0,
                DeviceId = 1,
                IsActive = false,
                IsDisabled = false
            });

            modelBuilder.Entity<Schedule>()
                .HasKey(p => new { p.ModuleId, p.Day, p.Hour, p.Minutes });

            modelBuilder.Entity<Schedule>().HasData(new Schedule { Day = 2, Hour = 8, Minutes = 30, ModuleId = 1, Status = 25 });
            modelBuilder.Entity<Schedule>().HasData(new Schedule { Day = 2, Hour = 9, Minutes = 00, ModuleId = 1, Status = 20 });
        }
    }
}
