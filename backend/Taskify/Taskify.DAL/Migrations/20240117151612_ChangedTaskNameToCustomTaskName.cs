using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taskify.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangedTaskNameToCustomTaskName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Sections_SectionId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Users_ResponsibleUserId",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks");

            migrationBuilder.RenameTable(
                name: "Tasks",
                newName: "CustomTasks");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_SectionId",
                table: "CustomTasks",
                newName: "IX_CustomTasks_SectionId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_ResponsibleUserId",
                table: "CustomTasks",
                newName: "IX_CustomTasks_ResponsibleUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomTasks",
                table: "CustomTasks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomTasks_Sections_SectionId",
                table: "CustomTasks",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomTasks_Users_ResponsibleUserId",
                table: "CustomTasks",
                column: "ResponsibleUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomTasks_Sections_SectionId",
                table: "CustomTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomTasks_Users_ResponsibleUserId",
                table: "CustomTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomTasks",
                table: "CustomTasks");

            migrationBuilder.RenameTable(
                name: "CustomTasks",
                newName: "Tasks");

            migrationBuilder.RenameIndex(
                name: "IX_CustomTasks_SectionId",
                table: "Tasks",
                newName: "IX_Tasks_SectionId");

            migrationBuilder.RenameIndex(
                name: "IX_CustomTasks_ResponsibleUserId",
                table: "Tasks",
                newName: "IX_Tasks_ResponsibleUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Sections_SectionId",
                table: "Tasks",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Users_ResponsibleUserId",
                table: "Tasks",
                column: "ResponsibleUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
