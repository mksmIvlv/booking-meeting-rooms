﻿// <auto-generated />
using System;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Models.BookingMeetingRoom", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateOnly>("DateMeeting")
                        .HasColumnType("date");

                    b.Property<TimeOnly>("EndTimeMeeting")
                        .HasColumnType("time without time zone");

                    b.Property<Guid>("MeetingRoomId")
                        .HasColumnType("uuid");

                    b.Property<TimeOnly>("StartTimeMeeting")
                        .HasColumnType("time without time zone");

                    b.HasKey("Id");

                    b.HasIndex("MeetingRoomId");

                    b.ToTable("BookingMeetingRooms");

                    b.HasData(
                        new
                        {
                            Id = new Guid("0892d620-d083-4b1e-93e7-d7faa9e2c765"),
                            DateMeeting = new DateOnly(2023, 10, 25),
                            EndTimeMeeting = new TimeOnly(11, 0, 0),
                            MeetingRoomId = new Guid("0df8a713-4406-4fc1-9b99-d4b57ea84ffe"),
                            StartTimeMeeting = new TimeOnly(10, 0, 0)
                        });
                });

            modelBuilder.Entity("Domain.Models.MeetingRoom", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("NameIndex");

                    b.ToTable("MeetingRooms");

                    b.HasData(
                        new
                        {
                            Id = new Guid("0df8a713-4406-4fc1-9b99-d4b57ea84ffe"),
                            Description = "Описание переговорной комнаты.",
                            Name = "Переговорная комната 1."
                        },
                        new
                        {
                            Id = new Guid("43cee076-a20a-44f7-8d34-dfb83e391dc7"),
                            Description = "Описание переговорной комнаты.",
                            Name = "Переговорная комната 2."
                        },
                        new
                        {
                            Id = new Guid("8817a810-2574-4652-8752-8fcec3ab9810"),
                            Description = "Описание переговорной комнаты.",
                            Name = "Переговорная комната 3."
                        },
                        new
                        {
                            Id = new Guid("039ae28c-071f-4817-8508-5464b2cc5309"),
                            Description = "Описание переговорной комнаты.",
                            Name = "Переговорная комната 4."
                        },
                        new
                        {
                            Id = new Guid("041eb44a-a077-4239-9719-9dfaf5591b3a"),
                            Description = "Описание переговорной комнаты.",
                            Name = "Переговорная комната 5."
                        });
                });

            modelBuilder.Entity("Domain.Models.BookingMeetingRoom", b =>
                {
                    b.HasOne("Domain.Models.MeetingRoom", null)
                        .WithMany("BookingMeetingRooms")
                        .HasForeignKey("MeetingRoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Models.MeetingRoom", b =>
                {
                    b.Navigation("BookingMeetingRooms");
                });
#pragma warning restore 612, 618
        }
    }
}
