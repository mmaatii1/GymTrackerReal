using GymTrackerApiReal.Dtos.SpecificExercise;

namespace GymTrackerApiReal.Dtos.CustomWorkout
{
    public class CustomWorkoutCreateUpdateDto
    {
        public List<SpecificExerciseReadDto> Exercises { get; set; }
        public DateTime DateOfWorkout { get; set; }
        public string Name { get; set; }
    }
}
