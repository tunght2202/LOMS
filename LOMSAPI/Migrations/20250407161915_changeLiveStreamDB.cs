using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LOMSAPI.Migrations
{
    /// <inheritdoc />
    public partial class changeLiveStreamDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "LiveStreams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "LiveStreams");
        }
    }
}
