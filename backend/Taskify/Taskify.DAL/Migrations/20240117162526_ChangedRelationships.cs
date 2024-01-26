using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taskify.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangedRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyMembers_CompanyMembersRoles_RoleId",
                table: "CompanyMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyMembersRoles_Companies_CompanyId",
                table: "CompanyMembersRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanyMembersRoles",
                table: "CompanyMembersRoles");

            migrationBuilder.RenameTable(
                name: "CompanyMembersRoles",
                newName: "CompanyMemberRoles");

            migrationBuilder.RenameIndex(
                name: "IX_CompanyMembersRoles_CompanyId",
                table: "CompanyMemberRoles",
                newName: "IX_CompanyMemberRoles_CompanyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanyMemberRoles",
                table: "CompanyMemberRoles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyMemberRoles_Companies_CompanyId",
                table: "CompanyMemberRoles",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_CompanyMemberRoles_Companies_CompanyId",
                table: "CompanyMemberRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyMembers_CompanyMemberRoles_RoleId",
                table: "CompanyMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanyMemberRoles",
                table: "CompanyMemberRoles");

            migrationBuilder.RenameTable(
                name: "CompanyMemberRoles",
                newName: "CompanyMembersRoles");

            migrationBuilder.RenameIndex(
                name: "IX_CompanyMemberRoles_CompanyId",
                table: "CompanyMembersRoles",
                newName: "IX_CompanyMembersRoles_CompanyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanyMembersRoles",
                table: "CompanyMembersRoles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyMembers_CompanyMembersRoles_RoleId",
                table: "CompanyMembers",
                column: "RoleId",
                principalTable: "CompanyMembersRoles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyMembersRoles_Companies_CompanyId",
                table: "CompanyMembersRoles",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
