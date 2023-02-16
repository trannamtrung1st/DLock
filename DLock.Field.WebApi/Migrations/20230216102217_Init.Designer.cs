﻿// <auto-generated />
using System;
using DLock.Field.WebApi.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DLock.Field.WebApi.Migrations
{
    [DbContext(typeof(FieldContext))]
    [Migration("20230216102217_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("DLock.Field.WebApi.Entities.FieldEntity", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsUnderMaintenance")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("MaintenanceTime")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Name");

                    b.ToTable("Field");

                    b.HasData(
                        new
                        {
                            Name = "Field 01",
                            IsUnderMaintenance = false
                        },
                        new
                        {
                            Name = "Field 02",
                            IsUnderMaintenance = false
                        },
                        new
                        {
                            Name = "Field 03",
                            IsUnderMaintenance = false
                        },
                        new
                        {
                            Name = "Field 04",
                            IsUnderMaintenance = false
                        },
                        new
                        {
                            Name = "Field 05",
                            IsUnderMaintenance = false
                        },
                        new
                        {
                            Name = "Field 06",
                            IsUnderMaintenance = false
                        },
                        new
                        {
                            Name = "Field 07",
                            IsUnderMaintenance = false
                        },
                        new
                        {
                            Name = "Field 08",
                            IsUnderMaintenance = false
                        },
                        new
                        {
                            Name = "Field 09",
                            IsUnderMaintenance = false
                        },
                        new
                        {
                            Name = "Field 10",
                            IsUnderMaintenance = false
                        },
                        new
                        {
                            Name = "Field 11",
                            IsUnderMaintenance = false
                        },
                        new
                        {
                            Name = "Field 12",
                            IsUnderMaintenance = false
                        },
                        new
                        {
                            Name = "Field 13",
                            IsUnderMaintenance = false
                        },
                        new
                        {
                            Name = "Field 14",
                            IsUnderMaintenance = false
                        },
                        new
                        {
                            Name = "Field 15",
                            IsUnderMaintenance = false
                        },
                        new
                        {
                            Name = "Field 16",
                            IsUnderMaintenance = false
                        },
                        new
                        {
                            Name = "Field 17",
                            IsUnderMaintenance = false
                        },
                        new
                        {
                            Name = "Field 18",
                            IsUnderMaintenance = false
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
