using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LOMSAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddPageIdtoUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PageId",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PageId",
                table: "Users");
        }
    }
}
