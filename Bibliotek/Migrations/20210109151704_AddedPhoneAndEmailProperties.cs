using Microsoft.EntityFrameworkCore.Migrations;

namespace Bibliotek.Migrations
{
    public partial class AddedPhoneAndEmailProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "rated",
                table: "Borrowings",
                newName: "Rated");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Borrowers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Borrowers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Borrowers");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Borrowers");

            migrationBuilder.RenameColumn(
                name: "Rated",
                table: "Borrowings",
                newName: "rated");
        }
    }
}
