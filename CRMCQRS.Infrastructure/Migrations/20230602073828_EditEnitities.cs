using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMCQRS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditEnitities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OfficeTimers");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Missions");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Missions");

            migrationBuilder.AddColumn<decimal>(
                name: "Salary",
                table: "AspNetUsers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "Sprint",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sprint", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sprint_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sprint_ProjectId",
                table: "Sprint",
                column: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sprint");

            migrationBuilder.DropColumn(
                name: "Salary",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Missions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Missions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "OfficeTimers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    Report = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficeTimers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfficeTimers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OfficeTimers_UserId",
                table: "OfficeTimers",
                column: "UserId");
        }
    }
}
