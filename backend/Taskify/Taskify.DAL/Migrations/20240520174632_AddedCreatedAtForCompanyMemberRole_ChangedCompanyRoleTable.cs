using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taskify.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedCreatedAtForCompanyMemberRole_ChangedCompanyRoleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyMembers_CompanyMemberRoles_RoleId",
                table: "CompanyMembers");

            migrationBuilder.DropTable(
                name: "CompanyMemberRoles");

            migrationBuilder.CreateTable(
                name: "CompanyRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CompanyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyRoles_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyRoles_CompanyId",
                table: "CompanyRoles",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyMembers_CompanyRoles_RoleId",
                table: "CompanyMembers",
                column: "RoleId",
                principalTable: "CompanyRoles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyMembers_CompanyRoles_RoleId",
                table: "CompanyMembers");

            migrationBuilder.DropTable(
                name: "CompanyRoles");

            migrationBuilder.CreateTable(
                name: "CompanyMemberRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CompanyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyMemberRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyMemberRoles_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyMemberRoles_CompanyId",
                table: "CompanyMemberRoles",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyMembers_CompanyMemberRoles_RoleId",
                table: "CompanyMembers",
                column: "RoleId",
                principalTable: "CompanyMemberRoles",
                principalColumn: "Id");
        }
    }
}
