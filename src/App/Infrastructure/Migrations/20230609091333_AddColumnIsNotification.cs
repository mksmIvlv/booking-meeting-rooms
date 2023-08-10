using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnIsNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsNotification",
                table: "BookingMeetingRooms",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsNotification",
                table: "BookingMeetingRooms");

            migrationBuilder.InsertData(
                table: "MeetingRooms",
                columns: new[] { "IdRoom", "DescriptionRoom", "NameRoom" },
                values: new object[,]
                {
                    { new Guid("63d0cfca-2905-4ce9-98fc-f65f0c5116f2"), "Описание переговорной комнаты.", "Переговорная комната 2." },
                    { new Guid("73e4e1b7-fc6f-42ed-8179-80e129c571d8"), "Описание переговорной комнаты.", "Переговорная комната 5." },
                    { new Guid("777757dc-e177-45ba-ac25-8e178fe5a0e2"), "Описание переговорной комнаты.", "Переговорная комната 3." },
                    { new Guid("7c0a2d75-b56b-4473-9cc5-e76e9591cc6d"), "Описание переговорной комнаты.", "Переговорная комната 1." },
                    { new Guid("96ef5235-77b1-4060-aa8d-f543c17deda4"), "Описание переговорной комнаты.", "Переговорная комната 4." }
                });

            migrationBuilder.InsertData(
                table: "BookingMeetingRooms",
                columns: new[] { "IdBooking", "DateMeeting", "EndTimeMeeting", "MeetingRoomId", "StartTimeMeeting" },
                values: new object[] { new Guid("a7af2f7e-4d47-4bcb-809e-461f95e3111a"), new DateOnly(2023, 10, 25), new TimeOnly(11, 0, 0), new Guid("7c0a2d75-b56b-4473-9cc5-e76e9591cc6d"), new TimeOnly(10, 0, 0) });
        }
    }
}
