using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reserveit.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBusinessImageUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Businesses",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Businesses");
        }
    }
}
