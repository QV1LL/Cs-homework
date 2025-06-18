using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpainChampionship.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCountOfGoalsAndCountOfSkippedGoalsToTeam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Teams",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("5b8719b6-59ef-4190-b299-be7c2fd193ae"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldDefaultValue: new Guid("d030d5e0-bb71-47e6-9026-adbf514d91a5"));

            migrationBuilder.AddColumn<int>(
                name: "CountOfGoals",
                table: "Teams",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CountOfSkippedGoals",
                table: "Teams",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountOfGoals",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "CountOfSkippedGoals",
                table: "Teams");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Teams",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("d030d5e0-bb71-47e6-9026-adbf514d91a5"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldDefaultValue: new Guid("5b8719b6-59ef-4190-b299-be7c2fd193ae"));
        }
    }
}
