using GymTrackerApiReal.Dtos.SpecificExercise;
using GymTrackerApiReal.Dtos.WorkoutPlan;

namespace GymTrackerApiReal.Dtos.CustomWorkout
{
    public class CustomWorkoutReadDto
    {
        public int Id { get; set; }
        public ICollection<SpecificExerciseReadDto> CustomWorkoutSpecificExercises { get; set; }
        public int WorkoutPlanId { get; set; }
        public DateTime DateOfWorkout { get; set; }
        public string Name { get; set; }
    }
}
