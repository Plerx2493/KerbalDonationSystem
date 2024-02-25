using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KDS.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameChannelPointRewardMember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RewardValue",
                table: "ChannelPointRewards",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "RewardPrompt",
                table: "ChannelPointRewards",
                newName: "Prompt");

            migrationBuilder.RenameColumn(
                name: "RewardName",
                table: "ChannelPointRewards",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "RewardIsEnabled",
                table: "ChannelPointRewards",
                newName: "IsEnabled");

            migrationBuilder.RenameColumn(
                name: "RewardCost",
                table: "ChannelPointRewards",
                newName: "Cost");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value",
                table: "ChannelPointRewards",
                newName: "RewardValue");

            migrationBuilder.RenameColumn(
                name: "Prompt",
                table: "ChannelPointRewards",
                newName: "RewardPrompt");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "ChannelPointRewards",
                newName: "RewardName");

            migrationBuilder.RenameColumn(
                name: "IsEnabled",
                table: "ChannelPointRewards",
                newName: "RewardIsEnabled");

            migrationBuilder.RenameColumn(
                name: "Cost",
                table: "ChannelPointRewards",
                newName: "RewardCost");
        }
    }
}
