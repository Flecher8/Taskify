using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taskify.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedStartDateTimeUtcInUserSubscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "StartDateTimeUtc",
                table: "UserSubscriptions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartDateTimeUtc",
                table: "UserSubscriptions");
        }
    }
}
