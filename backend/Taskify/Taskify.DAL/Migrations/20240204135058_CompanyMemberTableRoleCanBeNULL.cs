using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taskify.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CompanyMemberTableRoleCanBeNULL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyMembers_CompanyMemberRoles_RoleId",
                table: "CompanyMembers");

            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                table: "CompanyMembers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyMembers_CompanyMemberRoles_RoleId",
                table: "CompanyMembers",
                column: "RoleId",
                principalTable: "CompanyMemberRoles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyMembers_CompanyMemberRoles_RoleId",
                table: "CompanyMembers");

            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                table: "CompanyMembers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyMembers_CompanyMemberRoles_RoleId",
                table: "CompanyMembers",
                column: "RoleId",
                principalTable: "CompanyMemberRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
