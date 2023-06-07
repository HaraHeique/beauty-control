using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeautyControl.API.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreationStockMovimentsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SuppliersRatings_Employees_EmployeeId",
                schema: "Business",
                table: "SuppliersRatings");

            migrationBuilder.DropForeignKey(
                name: "FK_SuppliersRatings_Suppliers_SupplierId",
                schema: "Business",
                table: "SuppliersRatings");

            migrationBuilder.AlterColumn<bool>(
                name: "Active",
                schema: "Business",
                table: "Employees",
                type: "BIT",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.CreateTable(
                name: "StockMovements",
                schema: "Business",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(type: "INT", nullable: false),
                    Date = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    Process = table.Column<byte>(type: "TINYINT", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: true),
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockMovements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockMovements_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "Business",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockMovements_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "Business",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockMovements_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalSchema: "Business",
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_EmployeeId",
                schema: "Business",
                table: "StockMovements",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_ProductId",
                schema: "Business",
                table: "StockMovements",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_SupplierId",
                schema: "Business",
                table: "StockMovements",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_SuppliersRatings_Employees_EmployeeId",
                schema: "Business",
                table: "SuppliersRatings",
                column: "EmployeeId",
                principalSchema: "Business",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SuppliersRatings_Suppliers_SupplierId",
                schema: "Business",
                table: "SuppliersRatings",
                column: "SupplierId",
                principalSchema: "Business",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SuppliersRatings_Employees_EmployeeId",
                schema: "Business",
                table: "SuppliersRatings");

            migrationBuilder.DropForeignKey(
                name: "FK_SuppliersRatings_Suppliers_SupplierId",
                schema: "Business",
                table: "SuppliersRatings");

            migrationBuilder.DropTable(
                name: "StockMovements",
                schema: "Business");

            migrationBuilder.AlterColumn<bool>(
                name: "Active",
                schema: "Business",
                table: "Employees",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "BIT");

            migrationBuilder.AddForeignKey(
                name: "FK_SuppliersRatings_Employees_EmployeeId",
                schema: "Business",
                table: "SuppliersRatings",
                column: "EmployeeId",
                principalSchema: "Business",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SuppliersRatings_Suppliers_SupplierId",
                schema: "Business",
                table: "SuppliersRatings",
                column: "SupplierId",
                principalSchema: "Business",
                principalTable: "Suppliers",
                principalColumn: "Id");
        }
    }
}
