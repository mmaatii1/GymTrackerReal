using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymTrackerApiReal.Migrations
{
    /// <inheritdoc />
    public partial class addRelationBuilder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpecificExercises_CustomWorkouts_CustomWorkoutId",
                table: "SpecificExercises");

            migrationBuilder.AlterColumn<int>(
                name: "CustomWorkoutId",
                table: "SpecificExercises",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SpecificExercises_CustomWorkouts_CustomWorkoutId",
                table: "SpecificExercises",
                column: "CustomWorkoutId",
                principalTable: "CustomWorkouts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpecificExercises_CustomWorkouts_CustomWorkoutId",
                table: "SpecificExercises");

            migrationBuilder.AlterColumn<int>(
                name: "CustomWorkoutId",
                table: "SpecificExercises",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_SpecificExercises_CustomWorkouts_CustomWorkoutId",
                table: "SpecificExercises",
                column: "CustomWorkoutId",
                principalTable: "CustomWorkouts",
                principalColumn: "Id");
        }
    }
}
