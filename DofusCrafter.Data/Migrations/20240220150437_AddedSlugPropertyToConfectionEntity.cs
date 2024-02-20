using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DofusCrafter.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedSlugPropertyToConfectionEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Confections",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Confections");
        }
    }
}
