using Microsoft.EntityFrameworkCore.Migrations;

namespace Library.Migrations
{
    public partial class LastVersionBeforeDemo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Borrowings_InventoryID",
                table: "Borrowings");

            migrationBuilder.AddColumn<bool>(
                name: "rated",
                table: "Borrowings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Borrowings_InventoryID",
                table: "Borrowings",
                column: "InventoryID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Borrowings_InventoryID",
                table: "Borrowings");

            migrationBuilder.DropColumn(
                name: "rated",
                table: "Borrowings");

            migrationBuilder.CreateIndex(
                name: "IX_Borrowings_InventoryID",
                table: "Borrowings",
                column: "InventoryID",
                unique: true);
        }
    }
}
