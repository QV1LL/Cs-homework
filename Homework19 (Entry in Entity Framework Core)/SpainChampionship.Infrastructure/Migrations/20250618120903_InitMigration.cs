using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpainChampionship.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false, defaultValue: new Guid("d030d5e0-bb71-47e6-9026-adbf514d91a5")),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    City = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    CountOfVictories = table.Column<int>(type: "INTEGER", nullable: false),
                    CountOfDefeats = table.Column<int>(type: "INTEGER", nullable: false),
                    CountOfDraws = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Teams");
        }
    }
}
