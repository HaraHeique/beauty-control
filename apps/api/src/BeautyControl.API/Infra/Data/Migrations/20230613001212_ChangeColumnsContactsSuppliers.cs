using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeautyControl.API.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeColumnsContactsSuppliers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Telephone",
                schema: "Business",
                table: "Suppliers");

            migrationBuilder.AddColumn<string>(
                name: "Emails",
                schema: "Business",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Telephones",
                schema: "Business",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Emails",
                schema: "Business",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "Telephones",
                schema: "Business",
                table: "Suppliers");

            migrationBuilder.AddColumn<string>(
                name: "Telephone",
                schema: "Business",
                table: "Suppliers",
                type: "VARCHAR(16)",
                nullable: true);
        }
    }
}
