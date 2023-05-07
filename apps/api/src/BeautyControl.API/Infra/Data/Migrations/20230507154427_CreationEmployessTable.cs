using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeautyControl.API.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreationEmployessTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                schema: "Business",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(256)", nullable: false),
                    Email = table.Column<string>(type: "VARCHAR(256)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Position = table.Column<int>(type: "INT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees",
                schema: "Business");
        }
    }
}
