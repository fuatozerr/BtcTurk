using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BtcTurk.Migrations
{
    /// <inheritdoc />
    public partial class addedlogchannelname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChannelName",
                table: "NotificationLogs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChannelName",
                table: "NotificationLogs");
        }
    }
}
