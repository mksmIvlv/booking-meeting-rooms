using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNewTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingMeetingRooms_MeetingRooms_MeetingRoomId",
                table: "BookingMeetingRooms");

            migrationBuilder.RenameColumn(
                name: "NameRoom",
                table: "MeetingRooms",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "DescriptionRoom",
                table: "MeetingRooms",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "IdRoom",
                table: "MeetingRooms",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "MeetingRoomId",
                table: "BookingMeetingRooms",
                newName: "IdMeetingRoom");

            migrationBuilder.RenameColumn(
                name: "IdBooking",
                table: "BookingMeetingRooms",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_BookingMeetingRooms_MeetingRoomId",
                table: "BookingMeetingRooms",
                newName: "IX_BookingMeetingRooms_IdMeetingRoom");

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemInMeetingRoom",
                columns: table => new
                {
                    IdItem = table.Column<Guid>(type: "uuid", nullable: false),
                    IdMeetingRoom = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemPrice = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemInMeetingRoom", x => new { x.IdMeetingRoom, x.IdItem });
                    table.ForeignKey(
                        name: "FK_ItemInMeetingRoom_Items_IdItem",
                        column: x => x.IdItem,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemInMeetingRoom_MeetingRooms_IdMeetingRoom",
                        column: x => x.IdMeetingRoom,
                        principalTable: "MeetingRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemInMeetingRoom_IdItem",
                table: "ItemInMeetingRoom",
                column: "IdItem");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingMeetingRooms_MeetingRooms_IdMeetingRoom",
                table: "BookingMeetingRooms",
                column: "IdMeetingRoom",
                principalTable: "MeetingRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingMeetingRooms_MeetingRooms_IdMeetingRoom",
                table: "BookingMeetingRooms");

            migrationBuilder.DropTable(
                name: "ItemInMeetingRoom");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "MeetingRooms",
                newName: "NameRoom");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "MeetingRooms",
                newName: "DescriptionRoom");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "MeetingRooms",
                newName: "IdRoom");

            migrationBuilder.RenameColumn(
                name: "IdMeetingRoom",
                table: "BookingMeetingRooms",
                newName: "MeetingRoomId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "BookingMeetingRooms",
                newName: "IdBooking");

            migrationBuilder.RenameIndex(
                name: "IX_BookingMeetingRooms_IdMeetingRoom",
                table: "BookingMeetingRooms",
                newName: "IX_BookingMeetingRooms_MeetingRoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingMeetingRooms_MeetingRooms_MeetingRoomId",
                table: "BookingMeetingRooms",
                column: "MeetingRoomId",
                principalTable: "MeetingRooms",
                principalColumn: "IdRoom",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
