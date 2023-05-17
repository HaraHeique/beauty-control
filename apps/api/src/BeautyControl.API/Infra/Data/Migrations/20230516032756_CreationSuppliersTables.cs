using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeautyControl.API.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreationSuppliersTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Suppliers",
                schema: "Business",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "VARCHAR(128)", nullable: false),
                    Observation = table.Column<string>(type: "VARCHAR(MAX)", nullable: false),
                    Telephone = table.Column<string>(type: "VARCHAR(16)", nullable: false),
                    AverageRating = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "DATETIME", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SuppliersRatings",
                schema: "Business",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rating = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false),
                    Date = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuppliersRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuppliersRatings_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "Business",
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SuppliersRatings_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalSchema: "Business",
                        principalTable: "Suppliers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SuppliersRatings_EmployeeId",
                schema: "Business",
                table: "SuppliersRatings",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_SuppliersRatings_SupplierId",
                schema: "Business",
                table: "SuppliersRatings",
                column: "SupplierId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SuppliersRatings",
                schema: "Business");

            migrationBuilder.DropTable(
                name: "Suppliers",
                schema: "Business");
        }
    }
}
