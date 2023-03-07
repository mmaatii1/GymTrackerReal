using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymTrackerApiReal.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkoutPlanEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkoutPlanId",
                table: "Exercises",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkoutPlanId",
                table: "CustomWorkouts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WorkoutPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutPlans", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_WorkoutPlanId",
                table: "Exercises",
                column: "WorkoutPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomWorkouts_WorkoutPlanId",
                table: "CustomWorkouts",
                column: "WorkoutPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomWorkouts_WorkoutPlans_WorkoutPlanId",
                table: "CustomWorkouts",
                column: "WorkoutPlanId",
                principalTable: "WorkoutPlans",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_WorkoutPlans_WorkoutPlanId",
                table: "Exercises",
                column: "WorkoutPlanId",
                principalTable: "WorkoutPlans",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomWorkouts_WorkoutPlans_WorkoutPlanId",
                table: "CustomWorkouts");

            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_WorkoutPlans_WorkoutPlanId",
                table: "Exercises");

            migrationBuilder.DropTable(
                name: "WorkoutPlans");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_WorkoutPlanId",
                table: "Exercises");

            migrationBuilder.DropIndex(
                name: "IX_CustomWorkouts_WorkoutPlanId",
                table: "CustomWorkouts");

            migrationBuilder.DropColumn(
                name: "WorkoutPlanId",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "WorkoutPlanId",
                table: "CustomWorkouts");
        }
    }
}
