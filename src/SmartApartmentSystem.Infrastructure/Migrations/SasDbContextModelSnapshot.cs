﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SmartApartmentSystem.Infrastructure.Data;

namespace SmartApartmentSystem.Infrastructure.Migrations
{
    [DbContext(typeof(SasDbContext))]
    partial class SasDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2");

            modelBuilder.Entity("SmartApartmentSystem.Domain.Entity.Module", b =>
                {
                    b.Property<byte>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<byte?>("ExpectedStatus")
                        .HasColumnType("INTEGER");

                    b.Property<bool?>("IsDisabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Modules");

                    b.HasData(
                        new
                        {
                            Id = (byte)0,
                            ExpectedStatus = (byte)24,
                            IsDisabled = false,
                            Name = "Boiler"
                        },
                        new
                        {
                            Id = (byte)1,
                            ExpectedStatus = (byte)0,
                            IsDisabled = false,
                            Name = "Floor"
                        },
                        new
                        {
                            Id = (byte)2,
                            ExpectedStatus = (byte)1,
                            IsDisabled = false,
                            Name = "Water"
                        });
                });

            modelBuilder.Entity("SmartApartmentSystem.Domain.Entity.ModuleActual", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<byte?>("ActualStatus")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ChangeDate")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("ModuleId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ModuleId");

                    b.ToTable("ModuleActuals");
                });

            modelBuilder.Entity("SmartApartmentSystem.Domain.Entity.Schedule", b =>
                {
                    b.Property<int>("ScheduleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Friday")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("Hour")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("Minutes")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("ModuleId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Monday")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Saturday")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Sunday")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Thursday")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Tuesday")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Wednesday")
                        .HasColumnType("INTEGER");

                    b.HasKey("ScheduleId");

                    b.HasIndex("ModuleId");

                    b.ToTable("Schedules");
                });

            modelBuilder.Entity("SmartApartmentSystem.Domain.Entity.ModuleActual", b =>
                {
                    b.HasOne("SmartApartmentSystem.Domain.Entity.Module", "Module")
                        .WithMany("StatusHistory")
                        .HasForeignKey("ModuleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SmartApartmentSystem.Domain.Entity.Schedule", b =>
                {
                    b.HasOne("SmartApartmentSystem.Domain.Entity.Module", "Module")
                        .WithMany("Schedules")
                        .HasForeignKey("ModuleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
