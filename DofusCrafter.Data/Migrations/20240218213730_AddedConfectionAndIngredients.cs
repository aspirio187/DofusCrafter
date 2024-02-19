using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DofusCrafter.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedConfectionAndIngredients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Confections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ItemId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Confections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConfectionsIngredients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ConfectionId = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfectionsIngredients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfectionsIngredients_Confections_ConfectionId",
                        column: x => x.ConfectionId,
                        principalTable: "Confections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConfectionsIngredients_ConfectionId",
                table: "ConfectionsIngredients",
                column: "ConfectionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfectionsIngredients");

            migrationBuilder.DropTable(
                name: "Confections");
        }
    }
}
