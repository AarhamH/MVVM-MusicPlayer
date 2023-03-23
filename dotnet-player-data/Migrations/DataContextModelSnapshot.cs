﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using dotnet_player_data.Context;

#nullable disable

namespace dotnet_player_data.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true);

            modelBuilder.Entity("dotnet_player_data.Objects.PlayListObject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("PLTitle")
                        .HasMaxLength(40)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("PlayListObjects");
                });

            modelBuilder.Entity("dotnet_player_data.Objects.SongObjects", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ListID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Path")
                        .HasColumnType("TEXT");

                    b.Property<int?>("PlayListId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PlayListId");

                    b.ToTable("SongObjects");
                });

            modelBuilder.Entity("dotnet_player_data.Objects.SongObjects", b =>
                {
                    b.HasOne("dotnet_player_data.Objects.PlayListObject", "PlayList")
                        .WithMany("SongCollection")
                        .HasForeignKey("PlayListId");

                    b.Navigation("PlayList");
                });

            modelBuilder.Entity("dotnet_player_data.Objects.PlayListObject", b =>
                {
                    b.Navigation("SongCollection");
                });
#pragma warning restore 612, 618
        }
    }
}
