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
                .HasAnnotation("ProductVersion", "7.0.9")
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

                    b.Property<Guid>("IdMeetingRoom")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsNotification")
                        .HasColumnType("boolean");

                    b.Property<TimeOnly>("StartTimeMeeting")
                        .HasColumnType("time without time zone");

                    b.HasKey("Id");

                    b.HasIndex("IdMeetingRoom");

                    b.ToTable("BookingMeetingRooms");
                });

            modelBuilder.Entity("Domain.Models.Item", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("Domain.Models.ItemInMeetingRoom", b =>
                {
                    b.Property<Guid>("IdMeetingRoom")
                        .HasColumnType("uuid");

                    b.Property<Guid>("IdItem")
                        .HasColumnType("uuid");

                    b.Property<decimal?>("ItemPrice")
                        .HasColumnType("numeric");

                    b.HasKey("IdMeetingRoom", "IdItem");

                    b.HasIndex("IdItem");

                    b.ToTable("ItemInMeetingRoom", (string)null);
                });

            modelBuilder.Entity("Domain.Models.MeetingRoom", b =>
                {
                    b.Property<Guid>("Id")
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
                });

            modelBuilder.Entity("Domain.Models.BookingMeetingRoom", b =>
                {
                    b.HasOne("Domain.Models.MeetingRoom", null)
                        .WithMany("BookingMeetingRooms")
                        .HasForeignKey("IdMeetingRoom")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Models.ItemInMeetingRoom", b =>
                {
                    b.HasOne("Domain.Models.Item", "Item")
                        .WithMany()
                        .HasForeignKey("IdItem")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Models.MeetingRoom", "MeetingRoom")
                        .WithMany("ItemsInMeetingRooms")
                        .HasForeignKey("IdMeetingRoom")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");

                    b.Navigation("MeetingRoom");
                });

            modelBuilder.Entity("Domain.Models.MeetingRoom", b =>
                {
                    b.Navigation("BookingMeetingRooms");

                    b.Navigation("ItemsInMeetingRooms");
                });
#pragma warning restore 612, 618
        }
    }
}
