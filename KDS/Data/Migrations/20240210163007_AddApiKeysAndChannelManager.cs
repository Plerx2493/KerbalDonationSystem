using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KDS.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddApiKeysAndChannelManager : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApiKey",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ChannelConfigs",
                columns: table => new
                {
                    Id = table.Column<ulong>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ModeratorsAllowed = table.Column<bool>(type: "INTEGER", nullable: false),
                    ChannelName = table.Column<string>(type: "TEXT", nullable: false),
                    ChannelId = table.Column<ulong>(type: "INTEGER", nullable: false),
                    ChannelAccessToken = table.Column<string>(type: "TEXT", nullable: false),
                    ChannelRefreshToken = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelConfigs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Donations",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    user = table.Column<string>(type: "TEXT", nullable: false),
                    userId = table.Column<ulong>(type: "INTEGER", nullable: false),
                    channelId = table.Column<ulong>(type: "INTEGER", nullable: false),
                    amount = table.Column<int>(type: "INTEGER", nullable: false),
                    Value = table.Column<int>(type: "INTEGER", nullable: false),
                    createdAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ChannelPointRewards",
                columns: table => new
                {
                    Id = table.Column<ulong>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ChannelId = table.Column<ulong>(type: "INTEGER", nullable: false),
                    RewardId = table.Column<string>(type: "TEXT", nullable: false),
                    RewardName = table.Column<string>(type: "TEXT", nullable: false),
                    RewardPrompt = table.Column<string>(type: "TEXT", nullable: false),
                    RewardCost = table.Column<int>(type: "INTEGER", nullable: false),
                    RewardValue = table.Column<int>(type: "INTEGER", nullable: false),
                    RewardIsEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    ChannelConfigId = table.Column<ulong>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelPointRewards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChannelPointRewards_ChannelConfigs_ChannelConfigId",
                        column: x => x.ChannelConfigId,
                        principalTable: "ChannelConfigs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChannelPointRewards_ChannelConfigId",
                table: "ChannelPointRewards",
                column: "ChannelConfigId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChannelPointRewards");

            migrationBuilder.DropTable(
                name: "Donations");

            migrationBuilder.DropTable(
                name: "ChannelConfigs");

            migrationBuilder.DropColumn(
                name: "ApiKey",
                table: "AspNetUsers");
        }
    }
}
