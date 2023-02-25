using GymTrackerApi.Dtos.SpecificExercise;

namespace GymTrackerApi.Dtos.CustomWorkout
{
    public class CustomWorkoutCreateUpdateDto
    {
        public List<SpecificExerciseReadDto> Exercises { get; set; }
        public DateTime DateOfWorkout { get; set; }
        public string Name { get; set; }
    }
}
