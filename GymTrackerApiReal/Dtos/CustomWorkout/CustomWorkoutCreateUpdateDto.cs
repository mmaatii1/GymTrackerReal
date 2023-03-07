using GymTrackerApiReal.Dtos.SpecificExercise;

namespace GymTrackerApiReal.Dtos.CustomWorkout
{
    public class CustomWorkoutCreateUpdateDto
    {
        public IEnumerable<int> SpecificExercisesIds { get; set; }
        public DateTime DateOfWorkout { get; set; }
        public string Name { get; set; }
    }
}
