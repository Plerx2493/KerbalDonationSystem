using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KDS.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTwitchAuth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<ulong>(
                name: "TwitchId",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AspNetUsers_TwitchId",
                table: "AspNetUsers",
                column: "TwitchId");

            migrationBuilder.CreateTable(
                name: "TwitchAuths",
                columns: table => new
                {
                    Id = table.Column<ulong>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccessToken = table.Column<string>(type: "TEXT", nullable: false),
                    RefreshToken = table.Column<string>(type: "TEXT", nullable: false),
                    ExpiresAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    ChannelId = table.Column<ulong>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwitchAuths", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TwitchAuths_AspNetUsers_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "AspNetUsers",
                        principalColumn: "TwitchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TwitchAuths_ChannelId",
                table: "TwitchAuths",
                column: "ChannelId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TwitchAuths");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_AspNetUsers_TwitchId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TwitchId",
                table: "AspNetUsers");
        }
    }
}
