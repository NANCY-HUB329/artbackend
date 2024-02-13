﻿// <auto-generated />
using System;
using BidService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BidService.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240202114014_vnkasdjhbva")]
    partial class vnkasdjhbva
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BidService.Models.Bid", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ArtId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ArtName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("BidAmmount")
                        .HasColumnType("float");

                    b.Property<DateTime>("BidDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("BidderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("BidderName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Bids");
                });
#pragma warning restore 612, 618
        }
    }
}
