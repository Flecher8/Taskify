using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taskify.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomTaskCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomTasks_Sections_SectionId",
                table: "CustomTasks");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomTasks_Sections_SectionId",
                table: "CustomTasks",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomTasks_Sections_SectionId",
                table: "CustomTasks");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomTasks_Sections_SectionId",
                table: "CustomTasks",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
