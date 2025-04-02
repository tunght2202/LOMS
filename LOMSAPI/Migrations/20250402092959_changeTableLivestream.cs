using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LOMSAPI.Migrations
{
    /// <inheritdoc />
    public partial class changeTableLivestream : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "LiveStreams",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<bool>(
                name: "StatusDelete",
                table: "LiveStreams",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusDelete",
                table: "LiveStreams");

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "LiveStreams",
                type: "bit",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
