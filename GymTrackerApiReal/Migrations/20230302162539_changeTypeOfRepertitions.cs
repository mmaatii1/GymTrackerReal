using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymTrackerApiReal.Migrations
{
    /// <inheritdoc />
    public partial class changeTypeOfRepertitions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Repetitions",
                table: "SpecificExercises",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Repetitions",
                table: "SpecificExercises",
                type: "float",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
