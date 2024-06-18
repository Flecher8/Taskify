using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taskify.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCustomTaskForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomTasks_AspNetUsers_ResponsibleUserId",
                table: "CustomTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomTasks_Sections_SectionId",
                table: "CustomTasks");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomTasks_AspNetUsers_ResponsibleUserId",
                table: "CustomTasks",
                column: "ResponsibleUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomTasks_Sections_SectionId",
                table: "CustomTasks",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomTasks_AspNetUsers_ResponsibleUserId",
                table: "CustomTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomTasks_Sections_SectionId",
                table: "CustomTasks");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomTasks_AspNetUsers_ResponsibleUserId",
                table: "CustomTasks",
                column: "ResponsibleUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomTasks_Sections_SectionId",
                table: "CustomTasks",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id");
        }
    }
}
