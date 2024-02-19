using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DofusCrafter.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedQuantityToConfectionEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Confections",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Confections");
        }
    }
}
