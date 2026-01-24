using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mvcLab.Migrations
{
    /// <inheritdoc />
    public partial class instructor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Degree",
                table: "Instructors",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Degree",
                table: "Instructors",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
