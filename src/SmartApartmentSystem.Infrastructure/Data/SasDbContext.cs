using Microsoft.EntityFrameworkCore;
using SmartApartmentSystem.Application;
using SmartApartmentSystem.Domain.Entity;

namespace SmartApartmentSystem.Infrastructure.Data
{
    public sealed class SasDbContext : DbContext, ISasDb
    {
        public DbSet<Module> Modules { get; set; }
        public DbSet<ModuleActual> ModuleActuals { get; set; }
        public DbSet<Schedule> Schedules { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlite("Data Source=SasDb.db");
        //}
        public SasDbContext()
        : base()
        {
            //Database.EnsureCreated();
        }

        public SasDbContext(DbContextOptions options)
        : base(options)
        {
            //Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Module>()
                .HasMany(p => p.Schedules)
                .WithOne(b => b.Module)
                .HasForeignKey(p => p.ModuleId);

            modelBuilder.Entity<Module>()
                .HasMany(p => p.StatusHistory)
                .WithOne(b => b.Module)
                .HasForeignKey(p => p.ModuleId);

            modelBuilder.Entity<Module>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<ModuleActual>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Schedule>()
                .Property(e => e.ScheduleId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Module>().HasData(new Module
            {
                Id = 0,
                Name = "Boiler",
                ExpectedStatus = 24,
                IsDisabled = false
            },
            new Module
            {
                Id = 1,
                Name = "Floor",
                ExpectedStatus = 0,
                IsDisabled = false
            },
            new Module
            {
                Id = 2,
                Name = "Water",
                ExpectedStatus = 1,
                IsDisabled = false
            });
        }
    }
}
