using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMCQRS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSprintDbSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sprint_Projects_ProjectId",
                table: "Sprint");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sprint",
                table: "Sprint");

            migrationBuilder.RenameTable(
                name: "Sprint",
                newName: "Sprints");

            migrationBuilder.RenameIndex(
                name: "IX_Sprint_ProjectId",
                table: "Sprints",
                newName: "IX_Sprints_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sprints",
                table: "Sprints",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sprints_Projects_ProjectId",
                table: "Sprints",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sprints_Projects_ProjectId",
                table: "Sprints");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sprints",
                table: "Sprints");

            migrationBuilder.RenameTable(
                name: "Sprints",
                newName: "Sprint");

            migrationBuilder.RenameIndex(
                name: "IX_Sprints_ProjectId",
                table: "Sprint",
                newName: "IX_Sprint_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sprint",
                table: "Sprint",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sprint_Projects_ProjectId",
                table: "Sprint",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
