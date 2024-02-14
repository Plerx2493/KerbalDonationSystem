using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KDS.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTwitchAuthElevation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsElevated",
                table: "TwitchAuths",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsElevated",
                table: "TwitchAuths");
        }
    }
}
