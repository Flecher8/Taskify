using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taskify.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangedSubsciptionDateTimeName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EndDateTime",
                table: "UserSubscriptions",
                newName: "EndDateTimeUtc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EndDateTimeUtc",
                table: "UserSubscriptions",
                newName: "EndDateTime");
        }
    }
}
