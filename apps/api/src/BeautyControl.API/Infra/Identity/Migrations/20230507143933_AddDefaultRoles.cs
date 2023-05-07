using BeautyControl.API.Infra.Identity.Models;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable
namespace BeautyControl.API.Infra.Identity.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultRoles : Migration
    {
        const string AdminRole = UserRoles.AdminName;
        const string EmployeeRole = UserRoles.EmployeeName;

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@$"
                DECLARE @AdminRole VARCHAR(16) = '{AdminRole}';
                DECLARE @EmployeeRole VARCHAR(16) = '{EmployeeRole}';

                INSERT INTO 
	                [BeautyControl].[Identity].[AspNetRoles]([Name], [NormalizedName])
                VALUES 
	                (@AdminRole, UPPER(@AdminRole)),
	                (@EmployeeRole, UPPER(@EmployeeRole));
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@$"
                DELETE FROM [BeautyControl].[Identity].[AspNetRoles]
                WHERE [Name] IN ('{AdminRole}', '{EmployeeRole}');
            ");
        }
    }
}
