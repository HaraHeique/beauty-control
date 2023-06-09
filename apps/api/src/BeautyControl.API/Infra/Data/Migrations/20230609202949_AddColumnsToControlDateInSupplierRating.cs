using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeautyControl.API.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnsToControlDateInSupplierRating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                schema: "Business",
                table: "SuppliersRatings",
                newName: "LastRatingAt");

            migrationBuilder.AlterColumn<decimal>(
                name: "Rating",
                schema: "Business",
                table: "SuppliersRatings",
                type: "DECIMAL(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(18,2)")
                .Annotation("Relational:ColumnOrder", 4);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "Business",
                table: "SuppliersRatings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 1)
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastRatingAt",
                schema: "Business",
                table: "SuppliersRatings",
                type: "DATETIME",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME")
                .Annotation("Relational:ColumnOrder", 3);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstRatingAt",
                schema: "Business",
                table: "SuppliersRatings",
                type: "DATETIME",
                nullable: false,
                defaultValueSql: "GETDATE()")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                schema: "Business",
                table: "StockMovements",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                schema: "Business",
                table: "StockMovements",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstRatingAt",
                schema: "Business",
                table: "SuppliersRatings");

            migrationBuilder.RenameColumn(
                name: "LastRatingAt",
                schema: "Business",
                table: "SuppliersRatings",
                newName: "Date");

            migrationBuilder.AlterColumn<decimal>(
                name: "Rating",
                schema: "Business",
                table: "SuppliersRatings",
                type: "DECIMAL(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(18,2)")
                .OldAnnotation("Relational:ColumnOrder", 4);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "Business",
                table: "SuppliersRatings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("Relational:ColumnOrder", 1)
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                schema: "Business",
                table: "SuppliersRatings",
                type: "DATETIME",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME")
                .OldAnnotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                schema: "Business",
                table: "StockMovements",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                schema: "Business",
                table: "StockMovements",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
