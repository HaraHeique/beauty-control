using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeautyControl.API.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreationProductsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Business");

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "Business",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "VARCHAR(128)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", nullable: false),
                    ImageName = table.Column<string>(type: "VARCHAR(256)", nullable: true),
                    ImageUrl = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
                    Quantity = table.Column<int>(type: "INT", nullable: false),
                    RunningOutOfStock = table.Column<int>(type: "INT", nullable: false),
                    StatusStock = table.Column<byte>(type: "TINYINT", nullable: false),
                    Category = table.Column<short>(type: "SMALLINT", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "DATETIME", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products",
                schema: "Business");
        }
    }
}
