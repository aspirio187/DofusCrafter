﻿// <auto-generated />
using System;
using DofusCrafter.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DofusCrafter.Data.Migrations
{
    [DbContext(typeof(DofusCrafterDbContext))]
    partial class DofusCrafterDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.1");

            modelBuilder.Entity("DofusCrafter.Data.Entities.SaleEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ItemName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("SaleDate")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("UnitaryPrice")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Sales");
                });
#pragma warning restore 612, 618
        }
    }
}