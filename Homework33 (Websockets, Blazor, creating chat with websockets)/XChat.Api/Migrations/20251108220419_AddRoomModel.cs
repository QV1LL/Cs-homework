using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XChat.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddRoomModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoomsBannedUsers",
                columns: table => new
                {
                    BannedUsersId = table.Column<Guid>(type: "uuid", nullable: false),
                    Room1Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomsBannedUsers", x => new { x.BannedUsersId, x.Room1Id });
                    table.ForeignKey(
                        name: "FK_RoomsBannedUsers_Rooms_Room1Id",
                        column: x => x.Room1Id,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomsBannedUsers_Users_BannedUsersId",
                        column: x => x.BannedUsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoomsMessages",
                columns: table => new
                {
                    MessagesId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomsMessages", x => new { x.MessagesId, x.RoomId });
                    table.ForeignKey(
                        name: "FK_RoomsMessages_Messages_MessagesId",
                        column: x => x.MessagesId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomsMessages_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoomsUsers",
                columns: table => new
                {
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomsUsers", x => new { x.RoomId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_RoomsUsers_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomsUsers_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoomsBannedUsers_Room1Id",
                table: "RoomsBannedUsers",
                column: "Room1Id");

            migrationBuilder.CreateIndex(
                name: "IX_RoomsMessages_RoomId",
                table: "RoomsMessages",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomsUsers_UsersId",
                table: "RoomsUsers",
                column: "UsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomsBannedUsers");

            migrationBuilder.DropTable(
                name: "RoomsMessages");

            migrationBuilder.DropTable(
                name: "RoomsUsers");

            migrationBuilder.DropTable(
                name: "Rooms");
        }
    }
}
