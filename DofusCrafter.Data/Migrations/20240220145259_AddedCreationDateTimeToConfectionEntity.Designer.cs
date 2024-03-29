﻿// <auto-generated />
using System;
using DofusCrafter.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DofusCrafter.Data.Migrations
{
    [DbContext(typeof(DofusCrafterDbContext))]
    [Migration("20240220145259_AddedCreationDateTimeToConfectionEntity")]
    partial class AddedCreationDateTimeToConfectionEntity
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.1");

            modelBuilder.Entity("DofusCrafter.Data.Entities.ConfectionEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("ItemId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Confections");
                });

            modelBuilder.Entity("DofusCrafter.Data.Entities.ConfectionIngredientEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ConfectionId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Price")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ConfectionId");

                    b.ToTable("ConfectionsIngredients");
                });

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

            modelBuilder.Entity("DofusCrafter.Data.Entities.ConfectionIngredientEntity", b =>
                {
                    b.HasOne("DofusCrafter.Data.Entities.ConfectionEntity", "Confection")
                        .WithMany("ConfectionIngredients")
                        .HasForeignKey("ConfectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Confection");
                });

            modelBuilder.Entity("DofusCrafter.Data.Entities.ConfectionEntity", b =>
                {
                    b.Navigation("ConfectionIngredients");
                });
#pragma warning restore 612, 618
        }
    }
}
