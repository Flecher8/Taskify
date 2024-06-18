using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taskify.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyInvitationNotificationFKDeleteCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyInvitations_Notifications_NotificationId",
                table: "CompanyInvitations");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyInvitations_Notifications_NotificationId",
                table: "CompanyInvitations",
                column: "NotificationId",
                principalTable: "Notifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyInvitations_Notifications_NotificationId",
                table: "CompanyInvitations");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyInvitations_Notifications_NotificationId",
                table: "CompanyInvitations",
                column: "NotificationId",
                principalTable: "Notifications",
                principalColumn: "Id");
        }
    }
}
