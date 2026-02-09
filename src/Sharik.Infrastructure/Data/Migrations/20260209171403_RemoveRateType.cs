using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sharik.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRateType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Ratings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Ratings",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
