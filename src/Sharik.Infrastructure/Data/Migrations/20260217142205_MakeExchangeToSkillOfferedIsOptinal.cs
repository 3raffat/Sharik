using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sharik.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakeExchangeToSkillOfferedIsOptinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exchanges_Skills_SkillOfferedId",
                table: "Exchanges");

            migrationBuilder.AddForeignKey(
                name: "FK_Exchanges_Skills_SkillOfferedId",
                table: "Exchanges",
                column: "SkillOfferedId",
                principalTable: "Skills",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exchanges_Skills_SkillOfferedId",
                table: "Exchanges");

            migrationBuilder.AddForeignKey(
                name: "FK_Exchanges_Skills_SkillOfferedId",
                table: "Exchanges",
                column: "SkillOfferedId",
                principalTable: "Skills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
