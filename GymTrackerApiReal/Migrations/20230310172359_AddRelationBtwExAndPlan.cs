using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymTrackerApiReal.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationBtwExAndPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_WorkoutPlans_WorkoutPlanId",
                table: "Exercises");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_WorkoutPlanId",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "WorkoutPlanId",
                table: "Exercises");

            migrationBuilder.CreateTable(
                name: "ExerciseWorkoutPlan",
                columns: table => new
                {
                    ExercisesId = table.Column<int>(type: "int", nullable: false),
                    WorkoutPlansId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseWorkoutPlan", x => new { x.ExercisesId, x.WorkoutPlansId });
                    table.ForeignKey(
                        name: "FK_ExerciseWorkoutPlan_Exercises_ExercisesId",
                        column: x => x.ExercisesId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExerciseWorkoutPlan_WorkoutPlans_WorkoutPlansId",
                        column: x => x.WorkoutPlansId,
                        principalTable: "WorkoutPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseWorkoutPlan_WorkoutPlansId",
                table: "ExerciseWorkoutPlan",
                column: "WorkoutPlansId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExerciseWorkoutPlan");

            migrationBuilder.AddColumn<int>(
                name: "WorkoutPlanId",
                table: "Exercises",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_WorkoutPlanId",
                table: "Exercises",
                column: "WorkoutPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_WorkoutPlans_WorkoutPlanId",
                table: "Exercises",
                column: "WorkoutPlanId",
                principalTable: "WorkoutPlans",
                principalColumn: "Id");
        }
    }
}
