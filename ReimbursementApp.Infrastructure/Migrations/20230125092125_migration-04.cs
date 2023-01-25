using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReimbursementApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class migration04 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Employees",
                type: "varchar(1024)",
                unicode: false,
                maxLength: 1024,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(16)",
                oldMaxLength: 16);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Employees",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(1024)",
                oldUnicode: false,
                oldMaxLength: 1024);
        }
    }
}
